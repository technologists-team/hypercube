using System.Collections.Frozen;
using Hypercube.Client.Audio.Loading;
using Hypercube.Client.Utilities.Helpers;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Runtimes.Event;
using OpenTK.Audio.OpenAL;

namespace Hypercube.Client.Audio.Realisations.OpenAL;

/// <remarks>
/// For some reason, on my Windows 11 machine there is no <c>openal32.dll</c>
/// which should be a built-in library,
/// if you have this problem you need to download the <a href="https://openal.org/downloads/">library installer</a>.
/// </remarks>
public sealed class OpenAlAudioManager : IAudioManager, IEventSubscriber, IPostInject
{
    private const char DeviceExtensionDelimiter = ' ';

    private static readonly Logger LoggerOpenAl = LoggingManager.GetLogger("open_al");
    
    [Dependency] private readonly IAudioLoader _audioLoader = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private readonly Logger _logger = LoggingManager.GetLogger("audio_manager");

    private readonly HashSet<WeakReference<IAudioSource>> _loadedSources = new();
    private readonly Dictionary<AudioId, AudioHandler> _loadedAudio = new();
    // TODO: Make a centralized mechanism for hashing resources
    private readonly Dictionary<ResourcePath, AudioStream> _cachedStreams = new();
    
    private ALContext _context;
    private FrozenSet<string> _contextExtensions = FrozenSet<string>.Empty;
    
    private ALDevice _device;
    private FrozenSet<string> _deviceExtensions = FrozenSet<string>.Empty;
    
    private int _nextAudioId;

    private AudioId NextAudioId => new(_nextAudioId++);
    
    public void PostInject()
    {
        _eventBus.Subscribe(this, (ref RuntimeInitializationEvent _) =>
        {
            Initialize();
        });   
    }

    public void Initialize()
    {
        if (!LoadDevice())
            return;
        
        CreateContext();
    }
    
    private AudioStream CreateAudio(ResourcePath path, AudioSettings settings)
    {
        if (!AudioTypeHelper.TypesAssociation.TryGetValue(path.Extension.Remove(0, 1), out var audioType))
            throw new ArgumentException();

        using var stream = _resourceManager.ReadFileContent(path) ?? throw new InvalidOperationException();
        var audio = CreateAudio(stream, audioType, settings);
        return audio;
    }

    private AudioStream CreateAudio(Stream stream, AudioType audioType, AudioSettings settings)
    {
        var buffer = AL.GenBuffer();
        CatchAlError("Unable generate buffer");
        
        var data = _audioLoader.LoadAudioData(stream, audioType);
        
        unsafe
        {
            fixed (byte* pointer = data.Data.Span)
            {
                AL.BufferData(buffer, (ALFormat)data.Format, (nint)pointer, data.Data.Length, data.SampleRate);
                CatchAlError("Unable apply data for buffer");
            }
        }

        var id = NextAudioId;
        var handler = new AudioHandler(id, buffer);
        _loadedAudio.Add(id, handler);
        
        return new AudioStream(id, data.Length);
    }

    public IAudioSource CreateSource(ResourcePath path, AudioSettings settings)
    {
        var stream = GetAudio(path, settings);
        var source = CreateSource(stream);
        return source;
    }

    public IAudioSource CreateSource(AudioStream stream)
    {
        var source = AL.GenSource();
        var handler = _loadedAudio[stream.Id];

        AL.Source(source, ALSourcei.Buffer, handler.Buffer);
        CatchAlError("Source creation error");
            
        var audioSource = new AudioSource(source);
        _loadedSources.Add(new WeakReference<IAudioSource>(audioSource));

        return audioSource;
    }

    public AudioStream GetAudio(ResourcePath path, AudioSettings settings)
    {
        if (_cachedStreams.TryGetValue(path, out var result))
            return result;

        var audio = CreateAudio(path, settings);
        _cachedStreams[path] = audio;
        return audio;
    }

    public void PauseAll()
    {
        throw new NotImplementedException();
    }

    public void StopAll()
    {
        throw new NotImplementedException();
    }

    private bool LoadDevice()
    {
        // Select the "preferred device"
        _device = ALC.OpenDevice(null);
        if (CatchAlError("Failed loading open device"))
            return false;

        if (_device == nint.Zero)
        {
            LoggerOpenAl.Fatal($"Unable to open device {ALC.GetError(ALDevice.Null)}");
            return false;
        }

        LoadDeviceExtensions();
        return true;
    }

    private void LoadDeviceExtensions()
    {
        var extensions = ALC.GetString(_device, AlcGetString.Extensions);
        if (extensions is null)
        {
            LoggerOpenAl.EngineInfo("Unable to load device extensions");
            return;
        }

        var extensionSet = new HashSet<string>();
        foreach (var extension in extensions.Split(DeviceExtensionDelimiter))
        {
            extensionSet.Add(extension);
        }

        LoggerOpenAl.EngineInfo($"Loaded {extensionSet.Count} device extensions {extensions}");
        _deviceExtensions = extensionSet.ToFrozenSet();
    }

    private bool HasDeviceExtension(string extension)
    {
        return _deviceExtensions.Contains(extension);
    }

    private unsafe void CreateContext()
    {
        // TODO: Working with attribute list
        _context = ALC.CreateContext(_device, (int*)0);
        ALC.MakeContextCurrent(_context);

        LoadContextExtensions();
        
        if (CatchAlError())
            return;
        
        LoggerOpenAl.EngineInfo($"OpenAL Vendor: {AL.Get(ALGetString.Vendor)}");
        LoggerOpenAl.EngineInfo($"OpenAL Renderer: {AL.Get(ALGetString.Renderer)}");
        LoggerOpenAl.EngineInfo($"OpenAL Version: {AL.Get(ALGetString.Version)}");
        LoggerOpenAl.EngineInfo($"OpenAL EFX support: {HasDeviceExtension(ContextExtension.Efx)}");
    }

    private void LoadContextExtensions()
    {
        var extensions = ALC.GetString(ALDevice.Null, AlcGetString.Extensions);
        if (extensions is null)
        {
            LoggerOpenAl.EngineInfo("Unable to load context extensions");
            return;
        }
        
        var extensionSet = new HashSet<string>();
        foreach (var extension in extensions.Split(' '))
        {
            extensionSet.Add(extension);
        }

        LoggerOpenAl.EngineInfo($"Loaded {extensionSet.Count} context extensions {extensions}");
        _contextExtensions = extensionSet.ToFrozenSet();
    }
    
    private bool HasContextExtension(string extension)
    {
        return _contextExtensions.Contains(extension);
    }

    private readonly record struct AudioHandler(AudioId Id, int Buffer);
    
    private class AudioSource : IAudioSource
    {
        private readonly int _source;
        
        public IAudioGroup? Group { get; }
        
        public bool Playing => State == ALSourceState.Playing;

        public bool Looping
        {
            get => GetSource(ALSourceb.Looping);
            set => SetSource(ALSourceb.Looping, value);
        }

        public float Pitch
        {
            get => GetSource(ALSourcef.Pitch);
            set => SetSource(ALSourcef.Pitch, value);
        }

        public float Gain
        {
            get => GetSource(ALSourcef.Gain);
            set => SetSource(ALSourcef.Gain, value);
        }

        private ALSourceState State => (ALSourceState) AL.GetSource(_source, ALGetSourcei.SourceState);
        
        public AudioSource(int source)
        {
            _source = source;
        }
        
        public void Start()
        {
            AL.SourcePlay(_source);
        }

        public void Stop()
        {
            AL.SourceStop(_source);
        }

        public void Pause()
        {
            AL.SourcePause(_source);
        }
        
        public void Restart()
        {
            AL.SourceRewind(_source);
            AL.SourcePlay(_source);
        }
        
        public void Dispose()
        {
            AL.DeleteSource(_source);
        }

        private void SetSource(ALSourceb target, bool value)
        {
            AL.Source(_source, target, value);
        }
        
        private void SetSource(ALSourcef target, float value)
        {
            AL.Source(_source, target, value);
        }
        
        private bool GetSource(ALSourceb target)
        {
            return AL.GetSource(_source, target);
        }
        
        private float GetSource(ALSourcef target)
        {
            return AL.GetSource(_source, target);
        }
    }
    
    private static bool CatchAlError(string? message = null)
    {
        var error = AL.GetError();
        if (error == ALError.NoError)
            return false;
        
        var errorMessage = message is null ? string.Empty : $"message {message}";
        LoggerOpenAl.Fatal($"Handled OpenAL error {error} {errorMessage}");
        return true;
    }
}
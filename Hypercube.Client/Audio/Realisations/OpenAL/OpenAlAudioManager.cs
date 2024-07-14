using System.Collections.Frozen;
using Hypercube.Client.Audio.Events;
using Hypercube.Client.Audio.Loading;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Runtimes.Event;
using OpenTK.Audio.OpenAL;

namespace Hypercube.Client.Audio.Realisations.OpenAL;

/// <remarks>
/// For some reason, on my Windows 11 machine there is no <c>openal32.dll</c>
/// which should be a built-in library,
/// if you have this problem you need to download the <a href="https://openal.org/downloads/">library installer</a>.
/// </remarks>
public sealed partial class OpenAlAudioManager : IAudioManager, IEventSubscriber, IPostInject
{
    private const char DeviceExtensionDelimiter = ' ';

    [Dependency] private readonly IAudioLoader _audioLoader = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private readonly Logger _logger = LoggingManager.GetLogger("audio_manager");
    private readonly Logger _loggerOpenAl = LoggingManager.GetLogger("open_al");
    
    private readonly HashSet<WeakReference<IAudioSource>> _loadedSources = new();
    private readonly Dictionary<AudioId, AudioHandler> _loadedAudio = new();
    
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
        
        _logger.EngineInfo("Initialized");
        _eventBus.Raise(new AudioLibraryInitializedEvent());
    }

    private bool LoadDevice()
    {
        // Select the "preferred device"
        _device = ALC.OpenDevice(null);
        if (CatchAlError("Failed loading open device"))
            return false;

        if (_device == nint.Zero)
        {
            _loggerOpenAl.Fatal($"Unable to open device {ALC.GetError(ALDevice.Null)}");
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
            _loggerOpenAl.EngineInfo("Unable to load device extensions");
            return;
        }

        var extensionSet = new HashSet<string>();
        foreach (var extension in extensions.Split(DeviceExtensionDelimiter))
        {
            extensionSet.Add(extension);
        }

        _loggerOpenAl.EngineInfo($"Loaded {extensionSet.Count} device extensions {extensions}");
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
        
        _loggerOpenAl.EngineInfo($"OpenAL Vendor: {AL.Get(ALGetString.Vendor)}");
        _loggerOpenAl.EngineInfo($"OpenAL Renderer: {AL.Get(ALGetString.Renderer)}");
        _loggerOpenAl.EngineInfo($"OpenAL Version: {AL.Get(ALGetString.Version)}");
        _loggerOpenAl.EngineInfo($"OpenAL EFX support: {HasDeviceExtension(ContextExtension.Efx)}");
    }

    private void LoadContextExtensions()
    {
        var extensions = ALC.GetString(ALDevice.Null, AlcGetString.Extensions);
        if (extensions is null)
        {
            _loggerOpenAl.EngineInfo("Unable to load context extensions");
            return;
        }
        
        var extensionSet = new HashSet<string>();
        foreach (var extension in extensions.Split(' '))
        {
            extensionSet.Add(extension);
        }

        _loggerOpenAl.EngineInfo($"Loaded {extensionSet.Count} context extensions {extensions}");
        _contextExtensions = extensionSet.ToFrozenSet();
    }
    
    private bool HasContextExtension(string extension)
    {
        return _contextExtensions.Contains(extension);
    }
  
    private bool CatchAlError(string? message = null)
    {
        var error = AL.GetError();
        if (error == ALError.NoError)
            return false;
        
        var errorMessage = message is null ? string.Empty : $"message {message}";
        _loggerOpenAl.Fatal($"Handled OpenAL error {error} {errorMessage}");
        return true;
    }
}
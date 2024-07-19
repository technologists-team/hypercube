using Hypercube.Client.Audio.Settings;
using Hypercube.Client.Utilities.Helpers;
using Hypercube.Shared.Resources;
using OpenTK.Audio.OpenAL;

namespace Hypercube.Client.Audio.Realisations.OpenAL;

public sealed partial class OpenAlAudioManager
{
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
    
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws an exception if the specified extension is not declared in <see cref="AudioType"/>.
    /// </exception>
    public AudioStream CreateStream(ResourcePath path, IAudioSettings settings)
    {
        var audioType = AudioTypeHelper.GetAudioType(path.Extension);
        using var stream = _resourceLoader.ReadFileContent(path) ?? throw new InvalidOperationException();
        var audio = CreateAudio(stream, audioType, settings);
        return audio;
    }

    private AudioStream CreateAudio(Stream stream, AudioType audioType, IAudioSettings settings)
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
        var handler = new AudioHandler(buffer);
        
        _loadedAudio.Add(id, handler);
        
        var audioStream = new AudioStream(id, data.Format, data.Length, data.SampleRate);
        audioStream.OnDispose += OnAudioStreamDispose;

        return audioStream;
    }

    private void OnAudioStreamDispose(AudioStream stream)
    {
        if (!_loadedAudio.ContainsKey(stream.Id))
        {
            _logger.Warning($"Attempting to dispose a non-existent {stream.Id}");
            return;
        }

        _loadedAudio.Remove(stream.Id);
    }
}
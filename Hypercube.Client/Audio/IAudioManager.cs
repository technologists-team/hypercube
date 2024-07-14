using Hypercube.Shared.Resources;

namespace Hypercube.Client.Audio;

public interface IAudioManager
{
    IAudioSource CreateSource(ResourcePath path, AudioSettings settings);
    IAudioSource CreateSource(AudioStream stream);
    AudioStream GetAudio(ResourcePath path, AudioSettings settings);

    void Initialize();
    
    /// <summary>
    /// Pause all audio sources.
    /// </summary>
    void PauseAll();
    
    /// <summary>
    /// Stops all audio sources.
    /// </summary>
    void StopAll();
}
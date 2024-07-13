using Hypercube.Shared.Resources;

namespace Hypercube.Client.Audio;

public interface IAudioManager
{
    IAudio CreateAudio(ResourcePath path, IAudioSettings settings);
    IAudio CreateAudio(Stream stream, IAudioSettings settings);
    
    IAudio GetAudio(ResourcePath path, IAudioSettings settings);
    
    /// <summary>
    /// Pause all audio sources.
    /// </summary>
    void PauseAll();
    
    /// <summary>
    /// Stops all audio sources.
    /// </summary>
    void StopAll();
}
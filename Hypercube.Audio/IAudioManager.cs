using Hypercube.Audio.Settings;
using Hypercube.Resources;
using JetBrains.Annotations;

namespace Hypercube.Audio;

/// <summary>
/// Realization of audio library for work with audio devices,
/// as well as for work with <see cref="IAudioSource"/>.
/// </summary>
[PublicAPI]
public interface IAudioManager
{
    /// <summary>
    /// Audio library initialization method,
    /// automatically called when <see cref="RuntimeInitializationEvent"/>.
    /// </summary>
    /// <remarks>
    /// DO NOT USE IT WITHOUT KNOWING WHY YOU ARE DOING IT.
    /// </remarks>
    void Initialize();

    /// <summary>
    /// Starts all audio sources.
    /// </summary>
    /// <remarks>
    /// This method is more for testing than for actual tasks.
    /// It literally launches ALL loaded <see cref="IAudioSource"/>.
    /// </remarks>
    void StartAll();
    
    /// <summary>
    /// Pause all audio sources.
    /// </summary>
    void PauseAll();
    
    /// <summary>
    /// Stops all audio sources.
    /// </summary>
    void StopAll();
    
    IAudioSource CreateSource(AudioStream stream);
    AudioStream CreateStream(ResourcePath path, IAudioSettings settings);
}
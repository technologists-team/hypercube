namespace Hypercube.Client.Audio;

/// <summary>
/// Provides information about the loaded audio file to the external environment
/// and contains all the information necessary to create a <see cref="IAudioSource"/>.
/// </summary>
/// <seealso cref="IAudioManager.CreateSource(AudioStream)"/>
public sealed class AudioStream : IDisposable
{
    public readonly AudioId Id;
    public readonly AudioFormat AudioFormat;
    public readonly TimeSpan Length;
    public readonly int SampleRate;
    
    /// <summary>
    /// Event handled by <see cref="IAudioManager"/>
    /// to avoid memory leaks.
    /// </summary>
    public event Action<AudioStream>? OnDispose; 
    
    /// <summary>
    /// Turning this setting off may cause memory leaks, do it wisely.
    /// </summary>
    private readonly bool _shouldDisposeHandler = true;

    public AudioStream(AudioId id, AudioFormat audioFormat, TimeSpan length, int sampleRate)
    {
        Id = id;
        AudioFormat = audioFormat;
        Length = length;
        SampleRate = sampleRate;
    }

    public void Dispose()
    {
        if (!_shouldDisposeHandler)
            return;
        
        OnDispose?.Invoke(this);
    }
}
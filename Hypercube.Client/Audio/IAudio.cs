namespace Hypercube.Client.Audio;

public interface IAudio : IDisposable
{
    IAudioGroup? Group { get; }
    
    bool Looping { get; }
    bool Playing { get; }
    
    float Pitch { get; }
    float Gain { get; }
    
    void Start();
    void Stop();
    void Restart();
    void Pause();
}
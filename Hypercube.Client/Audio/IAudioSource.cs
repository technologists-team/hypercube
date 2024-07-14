namespace Hypercube.Client.Audio;

public interface IAudioSource : IDisposable
{
    IAudioGroup? Group { get; }
    bool Playing { get; }
    
    bool Looping { get; set; }
    float Pitch { get; set; }
    float Gain { get; set; }
    
    void Start();
    void Stop();
    void Restart();
    void Pause();
}
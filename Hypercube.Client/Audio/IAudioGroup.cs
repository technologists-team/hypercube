namespace Hypercube.Client.Audio;

public interface IAudioGroup
{
    float Gain { get; }

    void SetGain(float gain);

    void PauseAll();
    void StopAll();
}
using JetBrains.Annotations;

namespace Hypercube.Audio;

[PublicAPI]
public interface IAudioGroup
{
    float Gain { get; }

    void SetGain(float gain);

    void PauseAll();
    void StopAll();
}
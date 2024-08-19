using JetBrains.Annotations;

namespace Hypercube.Audio;

/// <summary>
/// Formats in which the audio library can read <see cref="AudioStream"/>.
/// some may require additional support from drivers and devices.
/// </summary>
[PublicAPI]
public enum AudioFormat
{
    Mono8 = 0x1100,
    Mono16 = 0x1101,
    Stereo8 = 0x1102,
    Stereo16 = 0x1103
}
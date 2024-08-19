using JetBrains.Annotations;

namespace Hypercube.Audio.Loading;

/// <summary>
/// Provides information about the read audio file to be loaded by <see cref="IAudioManager"/>.
/// </summary>
[PublicAPI]
public interface IAudioData
{
    AudioFormat Format { get; }
    ReadOnlyMemory<byte> Data { get; }
    int SampleRate { get; }
    TimeSpan Length { get; }
}
namespace Hypercube.Client.Audio.Loading;

/// <summary>
/// Provides information about the read audio file to be loaded by <see cref="IAudioManager"/>.
/// </summary>
public interface IAudioData
{
    AudioFormat Format { get; }
    ReadOnlyMemory<byte> Data { get; }
    int SampleRate { get; }
    TimeSpan Length { get; }
}
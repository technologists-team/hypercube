namespace Hypercube.Client.Audio;

public interface IAudioData
{
    AudioFormat Format { get; }
    ReadOnlyMemory<byte> Data { get; }
    int SampleRate { get; }
    TimeSpan Length { get; }
}
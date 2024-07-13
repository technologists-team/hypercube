namespace Hypercube.Client.Audio;

public sealed class AudioStream
{
    public readonly AudioId Id;
    public readonly TimeSpan Length;

    public AudioStream(AudioId id, TimeSpan length)
    {
        Id = id;
        Length = length;
    }
}
using Hypercube.Client.Audio.Readers.Wav;

namespace Hypercube.Client.Audio.Loading.TypeLoaders;

public sealed class AudioWavLoader : IAudioTypeLoader
{
    public IAudioData LoadAudioData(Stream stream)
    {
        using var reader = new AudioWavReader(stream);
        return reader.Read();
    }
}
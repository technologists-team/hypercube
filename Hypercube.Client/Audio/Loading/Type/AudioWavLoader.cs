using Hypercube.Client.Audio.Reader.Wav;

namespace Hypercube.Client.Audio.Loading.Type;

public sealed class AudioWavLoader : IAudioTypeLoader
{
    public IAudioData LoadAudioData(Stream stream)
    {
        var reader = new AudioWavReader(stream);
        return reader.Read();
    }
}
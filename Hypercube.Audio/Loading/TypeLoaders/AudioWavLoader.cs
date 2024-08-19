using Hypercube.Audio.Readers.Wav;
using JetBrains.Annotations;

namespace Hypercube.Audio.Loading.TypeLoaders;

[PublicAPI]
public sealed class AudioWavLoader : IAudioTypeLoader
{
    public IAudioData LoadAudioData(Stream stream)
    {
        using var reader = new AudioWavReader(stream);
        return reader.Read();
    }
}
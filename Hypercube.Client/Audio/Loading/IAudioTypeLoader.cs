using Hypercube.Client.Audio.Reader;

namespace Hypercube.Client.Audio.Loading;

public interface IAudioTypeLoader
{
    IAudioData LoadAudioData(Stream stream);
}
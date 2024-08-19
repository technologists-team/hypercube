using JetBrains.Annotations;

namespace Hypercube.Audio.Loading.TypeLoaders;

[PublicAPI]
public interface IAudioTypeLoader
{
    IAudioData LoadAudioData(Stream stream);
}
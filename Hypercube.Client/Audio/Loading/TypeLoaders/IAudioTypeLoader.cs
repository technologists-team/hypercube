namespace Hypercube.Client.Audio.Loading.TypeLoaders;

public interface IAudioTypeLoader
{
    IAudioData LoadAudioData(Stream stream);
}
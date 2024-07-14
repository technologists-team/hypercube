namespace Hypercube.Client.Audio.Loading.Type;

public interface IAudioTypeLoader
{
    IAudioData LoadAudioData(Stream stream);
}
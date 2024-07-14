namespace Hypercube.Client.Audio.Loading;

public interface IAudioLoader
{
    IAudioData LoadAudioData(Stream stream, AudioType type);
}
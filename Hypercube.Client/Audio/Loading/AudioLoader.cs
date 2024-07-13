using System.Collections.Frozen;
using Hypercube.Client.Audio.Loading.Type;

namespace Hypercube.Client.Audio.Loading;

public sealed class AudioLoader : IAudioLoader
{
    private readonly FrozenDictionary<AudioType, IAudioTypeLoader> _loaders = new Dictionary<AudioType, IAudioTypeLoader>
    {
        { AudioType.Wav, new AudioWavLoader() }
    }.ToFrozenDictionary();


    public IAudioData LoadAudioData(Stream stream, AudioType type)
    {
        return _loaders[type].LoadAudioData(stream);
    }
}
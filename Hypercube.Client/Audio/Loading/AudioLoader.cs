using System.Collections.Frozen;
using Hypercube.Client.Audio.Loading.Exceptions;
using Hypercube.Client.Audio.Loading.TypeLoaders;

namespace Hypercube.Client.Audio.Loading;

public sealed class AudioLoader : IAudioLoader
{
    /// <summary>
    /// Registration location of all <see cref="IAudioTypeLoader"/>.
    /// </summary>
    private readonly FrozenDictionary<AudioType, IAudioTypeLoader> _loaders = new Dictionary<AudioType, IAudioTypeLoader>
    {
        { AudioType.Wav, new AudioWavLoader() }
    }.ToFrozenDictionary();


    public IAudioData LoadAudioData(Stream stream, AudioType type)
    {
        if (!_loaders.TryGetValue(type, out var value))
            throw new UnregisteredLoaderException(type);
            
        return value.LoadAudioData(stream);
    }
}
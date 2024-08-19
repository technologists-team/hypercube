using System.Collections.Frozen;
using Hypercube.Audio.Exceptions;
using Hypercube.Audio.Loading.TypeLoaders;
using JetBrains.Annotations;

namespace Hypercube.Audio.Loading;

[PublicAPI]
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
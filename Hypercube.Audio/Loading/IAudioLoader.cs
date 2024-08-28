using Hypercube.Audio.Exceptions;
using Hypercube.Audio.Loading.TypeLoaders;
using JetBrains.Annotations;

namespace Hypercube.Audio.Loading;

/// <summary>
/// A router for audio files that selects a <see cref="IAudioLoader"/>
/// based on <see cref="AudioType"/>, and invoke it.
/// </summary>
[PublicAPI]
public interface IAudioLoader
{
    /// <summary>
    /// A method to call the <see cref="IAudioTypeLoader"/> to load the required <see cref="Stream"/>.
    /// </summary>
    /// <remarks>
    /// This operation dispose the <see cref="Stream"/>.
    /// </remarks>
    /// <exception cref="UnregisteredLoaderException">
    /// Throws an exception
    /// if the specified type does not have its own <see cref="IAudioTypeLoader"/>
    /// or the loader is not registered.
    /// </exception>
    IAudioData LoadAudioData(Stream stream, AudioType type);
}
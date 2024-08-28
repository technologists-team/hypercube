using JetBrains.Annotations;

namespace Hypercube.Audio.Exceptions;

[PublicAPI]
public sealed class UnregisteredLoaderException : Exception
{
    public UnregisteredLoaderException(AudioType type) : base($"Loader for type {type} not registered")
    {
        
    }
}
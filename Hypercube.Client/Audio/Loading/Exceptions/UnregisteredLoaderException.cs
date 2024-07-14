namespace Hypercube.Client.Audio.Loading.Exceptions;

public sealed class UnregisteredLoaderException : System.Exception
{
    public UnregisteredLoaderException(AudioType type) : base(
        $"Loader for type {type} not registered")
    {
        
    }
}
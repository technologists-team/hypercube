namespace Hypercube.Graphics.Core.Exceptions;

[Serializable]
public class GraphicException : Exception
{
    public GraphicException(string message) : base(message)
    {
    }
}
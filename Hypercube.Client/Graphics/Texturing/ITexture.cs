namespace Hypercube.Client.Graphics.Texturing;

public interface ITexture
{
    int Width { get; }
    int Height { get; }
    byte[] Data { get; }
}
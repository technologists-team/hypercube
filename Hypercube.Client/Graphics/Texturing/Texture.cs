using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Texturing;

public readonly struct Texture(Vector2Int size, byte[] data) : ITexture
{
    public int Width => size.X;
    public int Height => size.Y;
    public byte[] Data => data;
}
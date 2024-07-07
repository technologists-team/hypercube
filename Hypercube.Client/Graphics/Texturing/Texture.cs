using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Texturing;

public readonly struct Texture(Vector2Int size, int[,] data) : ITexture
{
    public int Width => size.X;
    public int Height => size.Y;
    public int[,] Data => (int[,])data.Clone();
}
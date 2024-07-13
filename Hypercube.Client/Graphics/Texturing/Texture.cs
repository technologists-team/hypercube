using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing;

public readonly struct Texture(ResourcePath path, Vector2Int size, byte[] data) : ITexture
{
    public ResourcePath Path { get; } = path;
    public int Width { get; } = size.X;
    public int Height { get; } = size.Y;
    public byte[] Data { get; } = data;
    public Box2 QuadCrateTranslated(Vector2 position)
    {
        return new Box2(position, position + (Vector2)size);
    }
}
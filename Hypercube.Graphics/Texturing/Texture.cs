using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing;

[PublicAPI]
public readonly struct Texture : ITexture
{
    public const int PixelPerUnit = 32;

    public ResourcePath Path { get; }
    public Vector2Int Size { get; }

    public byte[] Data { get; }
    
    public int Width => Size.X;
    public int Height => Size.Y;
    
    public Box2 Quad
    {
        get
        {
            var size = (Vector2)Size / PixelPerUnit;
            return new Box2(-size / 2, size / 2);
        }
    }

    public Texture(ResourcePath path, Vector2Int size, byte[] data)
    {
        Path = path;
        Size = size;
        Data = data;
    }
}
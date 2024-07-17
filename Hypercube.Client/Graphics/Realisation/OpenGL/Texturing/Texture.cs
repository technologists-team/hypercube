using Hypercube.Client.Graphics.Texturing;
using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Texturing;

public readonly struct Texture : ITexture
{
    public const int PixelPerUnit = 32;
    
    public ResourcePath Path { get; }
    public Vector2Int Size { get; }
    public byte[] Data { get; }

    public int Width => Size.X;
    public int Height => Size.Y;
    
    public Texture(ResourcePath path, Vector2Int size, byte[] data)
    {
        Path = path;
        Size = size;
        Data = data;
    }
    
    public Box2 QuadCrateTranslated()
    {
        var size = (Vector2)Size / PixelPerUnit;
        return new Box2(-size / 2, size / 2);
    }
}
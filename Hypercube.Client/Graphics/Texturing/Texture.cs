using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Texturing;

public readonly struct Texture(Vector2Int size, byte[] data) : ITexture
{
    public readonly Vector2Int Size = size; 
    
    public int Width => size.X;
    public int Height => size.Y;
    public byte[] Data => data;
    
    public Box2 QuadCrateTranslated(Vector2 position)
    {
        return new Box2(position, position + (Vector2)Size);
    }
}
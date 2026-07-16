using Hypercube.Graphics.Resources.Textures;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics;

public struct Sprite
{
    public Rect2 Uv;
    public Color Color;
    public Vector2 Pivot;
    public TextureId Texture;
}

public enum SpriteScaleMode : byte
{
    
}

public enum SpriteBlendMode : byte
{
    
}
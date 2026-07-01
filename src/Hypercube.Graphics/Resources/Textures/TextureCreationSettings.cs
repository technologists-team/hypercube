using Hypercube.Graphics.Resources.Textures.Types;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Resources.Textures;

public struct TextureCreationSettings
{
    public Vector2i Size;
    public int Depth;

    public TextureType Type;
    public TextureFormat Format;
    
    public TextureFilter MinFilter;
    public TextureFilter MagFilter;
    
    public TextureWrap WrapU;
    public TextureWrap WrapV;
    public TextureWrap WrapW;
    
    public bool GenerateMipmaps;
}
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Resources.Textures;

public readonly struct TextureMetadata
{
    public Vector2i Size { get; init; }
    
    public int Depth { get; init; }
    
    public TextureType Type { get; init; }
    
    public TextureFormat Format { get; init; }
    
    // Not realized yet
    public int MipmapLevels { get; init; }
    
    public bool HasMipmaps => MipmapLevels > 1;
}

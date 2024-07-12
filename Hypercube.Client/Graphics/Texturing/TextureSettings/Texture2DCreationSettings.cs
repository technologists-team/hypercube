using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public class Texture2DCreationSettings : ITextureCreationSettings
{
    public Texture2DCreationSettings(
        TextureTarget textureTarget, 
        HashSet<TextureParameter> parameters, 
        PixelInternalFormat pixelInternalFormat, 
        int level, 
        int border, 
        PixelFormat pixelFormat, 
        PixelType pixelType,
        bool flip = true)
    {
        TextureTarget = textureTarget;
        Parameters = parameters;
        PixelInternalFormat = pixelInternalFormat;
        Level = level;
        Border = border;
        PixelFormat = pixelFormat;
        PixelType = pixelType;
        Flip = flip;
    }

    public Texture2DCreationSettings()
    {
        TextureTarget = TextureTarget.Texture2D;
        Parameters = new()
        {
            new TextureParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat),
            new TextureParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat),
            new TextureParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear),
            new TextureParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear),
        };
        PixelInternalFormat = PixelInternalFormat.Rgba;
        Level = 0;
        Border = 0;
        PixelFormat = PixelFormat.Rgba;
        PixelType = PixelType.UnsignedByte;
        Flip = true;
    }
    public TextureTarget TextureTarget { get; }
    public HashSet<TextureParameter> Parameters { get; }
    public PixelInternalFormat PixelInternalFormat { get; }
    public int Level { get; }
    public int Border { get; }
    public PixelFormat PixelFormat { get; }
    public PixelType PixelType { get; }
    
    public bool Flip { get; }
}
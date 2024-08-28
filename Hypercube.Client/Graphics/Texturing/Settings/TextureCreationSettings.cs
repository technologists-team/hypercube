using Hypercube.Graphics.Texturing.Settings;
using OpenToolkit.Graphics.OpenGL4;
using PixelFormat = Hypercube.Graphics.Texturing.Parameters.PixelFormat;
using PixelInternalFormat = Hypercube.Graphics.Texturing.Parameters.PixelInternalFormat;
using PixelType = Hypercube.Graphics.Texturing.Parameters.PixelType;
using TextureParameterName = Hypercube.Graphics.Texturing.Parameters.TextureParameterName;
using TextureTarget = Hypercube.Graphics.Texturing.Parameters.TextureTarget;

namespace Hypercube.Client.Graphics.Texturing.Settings;

public struct TextureCreationSettings : ITextureCreationSettings
{
    public TextureCreationSettings(
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

    public TextureCreationSettings()
    {
        TextureTarget = TextureTarget.Texture2D;
        Parameters =
        [
            new TextureParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat),
            new TextureParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat),
            new TextureParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest),
            new TextureParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest)
        ];
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
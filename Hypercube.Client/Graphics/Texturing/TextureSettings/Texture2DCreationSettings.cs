using OpenToolkit.Graphics.OpenGL4;
using PixelFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelFormat;
using PixelInternalFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelInternalFormat;
using PixelType = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelType;
using TextureParameterName = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureParameterName;
using TextureTarget = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureTarget;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public struct Texture2DCreationSettings : ITextureCreationSettings
{
    public Texture2DCreationSettings(
        TextureTarget textureTarget, 
        Dictionary<TextureParameterName, int> parameters, 
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
        Flipped = flip;
    }

    public Texture2DCreationSettings()
    {
        TextureTarget = TextureTarget.Texture2D;
        Parameters = new()
        {
            { TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat },
            { TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat },
            { TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest },
            { TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest },
        };
        PixelInternalFormat = PixelInternalFormat.Rgba;
        Level = 0;
        Border = 0;
        PixelFormat = PixelFormat.Rgba;
        PixelType = PixelType.UnsignedByte;
        Flipped = true;
    }
    public TextureTarget TextureTarget { get; }
    public Dictionary<TextureParameterName, int> Parameters { get; }
    public PixelInternalFormat PixelInternalFormat { get; }
    public int Level { get; }
    public int Border { get; }
    public PixelFormat PixelFormat { get; }
    public PixelType PixelType { get; }
    
    public bool Flipped { get; init; }
}
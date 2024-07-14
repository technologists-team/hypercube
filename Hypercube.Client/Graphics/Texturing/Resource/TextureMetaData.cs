using System.Runtime.Serialization;
using Hypercube.Client.Graphics.Texturing.TextureSettings;
using OpenToolkit.Graphics.OpenGL4;
using PixelFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelFormat;
using PixelInternalFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelInternalFormat;
using PixelType = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelType;
using TextureParameterName = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureParameterName;
using TextureTarget = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureTarget;

namespace Hypercube.Client.Graphics.Texturing.Resource;

[Serializable]
public sealed class TextureMetaData : ITextureCreationSettings
{
    [DataMember(Name = "target")]
    public TextureTarget TextureTarget { get; set; } = TextureTarget.Texture2D;
    
    [DataMember(Name = "parameters")]
    public Dictionary<TextureParameterName, int> Parameters { get; set; } = new()
    {
        { TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat },
        { TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat },
        { TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest },
        { TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest },
    };

    [DataMember(Name = "pixelInternalFormat")]
    public PixelInternalFormat PixelInternalFormat { get; set; } = PixelInternalFormat.Rgba;

    [DataMember(Name = "pixelFormat")]
    public PixelFormat PixelFormat { get; set; } = PixelFormat.Rgba;

    [DataMember(Name = "pixelType")]
    public PixelType PixelType { get; set; } = PixelType.UnsignedByte;
    
    [DataMember(Name = "level")]
    public int Level { get; set; }

    [DataMember(Name = "border")]
    public int Border { get; set; }

    [DataMember(Name = "flipped")]
    public bool Flipped { get; set; } = true;
}
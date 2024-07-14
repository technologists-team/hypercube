using System.Runtime.Serialization;
using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters;

namespace Hypercube.Client.Graphics.Texturing.Resource;

[Serializable]
public sealed class TextureMetaData : ITextureCreationSettings
{
    [DataMember(Name = "target")]
    public TextureTarget TextureTarget { get; set; } = TextureTarget.Texture2D;
    
    [DataMember(Name = "parameters")]
    public HashSet<TextureParameter> Parameters { get; set; } = new();

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
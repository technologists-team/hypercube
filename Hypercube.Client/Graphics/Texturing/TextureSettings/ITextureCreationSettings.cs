using OpenToolkit.Graphics.OpenGL4;
using PixelInternalFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelInternalFormat;
using PixelFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelFormat;
using PixelType = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelType;
using TextureTarget = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureTarget;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public interface ITextureCreationSettings
{
    TextureTarget TextureTarget { get; }
    HashSet<TextureParameter> Parameters { get; }
    PixelInternalFormat PixelInternalFormat { get; }
    int Level { get; }
    int Border { get; }
    PixelFormat PixelFormat { get; }
    PixelType PixelType { get; }
    
    bool Flip { get; }
}
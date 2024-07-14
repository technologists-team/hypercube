using Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters;

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
    bool Flipped { get; }
}
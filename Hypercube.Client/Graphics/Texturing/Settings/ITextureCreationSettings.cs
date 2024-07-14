using Hypercube.Client.Graphics.Texturing.Parameters;

namespace Hypercube.Client.Graphics.Texturing.Settings;

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
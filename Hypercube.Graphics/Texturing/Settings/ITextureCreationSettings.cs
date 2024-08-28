using Hypercube.Graphics.Texturing.Parameters;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing.Settings;

[PublicAPI]
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
using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureManager
{
    ITexture Create(ResourcePath path);
    /// <summary>
    /// Creates ITexture, allows to set flipping mode
    /// </summary>
    /// <param name="path">Path to image</param>
    /// <param name="doFlip"><a href="https://www.youtube.com/watch?v=WQuL95_ckDo">DO FLIP</a></param>
    /// <returns>ITexture</returns>
    ITexture Create(ResourcePath path, bool doFlip);
    
    ITextureHandle CreateHandler(ResourcePath path, ITextureCreationSettings settings);
    ITextureHandle CreateHandler(ITexture texture, ITextureCreationSettings settings);
}
namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureManager
{
    ITexture Create(string path);
    /// <summary>
    /// Creates ITexture, allows to set flipping mode
    /// </summary>
    /// <param name="path">Path to image</param>
    /// <param name="doFlip"><a href="https://www.youtube.com/watch?v=WQuL95_ckDo">DO FLIP</a></param>
    /// <returns>ITexture</returns>
    ITexture Create(string path, bool doFlip);
    
    ITextureHandle CreateHandler(string path);
    ITextureHandle CreateHandler(ITexture texture);
}
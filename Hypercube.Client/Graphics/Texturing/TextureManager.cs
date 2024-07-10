using StbImageSharp;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureManager : ITextureManager
{
    public TextureManager()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }
    
    public ITexture Create(string path)
    {
        using var stream = File.OpenRead(path);
        return Create(ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha));
    }

    public ITexture Create(string path, bool doFlip)
    {
        if (doFlip)
            StbImage.stbi_set_flip_vertically_on_load(0);
        var texture = Create(path);
        
        StbImage.stbi_set_flip_vertically_on_load(1);
        return texture;
    }

    public ITextureHandle CreateHandler(ITexture texture)
    {
        return new TextureHandle(texture);
    }
    
    public ITextureHandle CreateHandler(string path)
    {
        return CreateHandler(Create(path));
    }

    private ITexture Create(ImageResult image)
    {
        return new Texture((image.Width, image.Height), image.Data);
    }
}
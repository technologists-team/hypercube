using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using StbImageSharp;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureManager : ITextureManager
{
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    public TextureManager()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }
    
    public ITexture Create(ResourcePath path)
    {
        return Create(ImageResult.FromStream(_resourceManager.ReadFileContent(path) ?? throw new FileNotFoundException(), ColorComponents.RedGreenBlueAlpha));
    }

    public ITexture Create(ResourcePath path, bool doFlip)
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
    
    public ITextureHandle CreateHandler(ResourcePath path)
    {
        return CreateHandler(Create(path));
    }

    private ITexture Create(ImageResult image)
    {
        return new Texture((image.Width, image.Height), image.Data);
    }
}
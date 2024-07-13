using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using StbImageSharp;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureManager : ITextureManager
{
    [Dependency] private readonly IResourceManager _resourceManager = default!;

    private readonly Dictionary<ResourcePath, ITexture> _cachedTextures = new();
    private readonly Dictionary<ITexture, ITextureHandle> _cachedHandles = new();
    
    public TextureManager()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }
    
    public ITexture Create(ResourcePath path)
    {
        using var stream = _resourceManager.ReadFileContent(path) ?? throw new FileNotFoundException();
        var texture = Create(ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha));
        return texture;
    }

    public ITexture Create(ResourcePath path, bool doFlip)
    {
        // TODO: Fix flip possible problems here
        if (_cachedTextures.TryGetValue(path, out var result))
            return result;
        
        if (doFlip)
            StbImage.stbi_set_flip_vertically_on_load(0);
        
        var texture = Create(path);
        
        StbImage.stbi_set_flip_vertically_on_load(1);
        return _cachedTextures[path] = texture;
    }

    public ITextureHandle GetHandler(ITexture texture)
    {
        if (_cachedHandles.TryGetValue(texture, out var result))
            return result;
            
        return _cachedHandles[texture] = new TextureHandle(texture);
    }
    
    public ITextureHandle GetHandler(ResourcePath path)
    {
        return GetHandler(Create(path));
    }

    private ITexture Create(ImageResult image)
    {
        return new Texture((image.Width, image.Height), image.Data);
    }
}
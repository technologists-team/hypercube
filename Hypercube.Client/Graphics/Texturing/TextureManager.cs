using Hypercube.Client.Graphics.Texturing.TextureSettings;
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
        if (_cachedTextures.TryGetValue(path, out var value))
            return value;
        
        using var stream = _resourceManager.ReadFileContent(path) ?? throw new FileNotFoundException();
        var texture = Create(ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha));
        _cachedTextures[path] = texture;
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

    public ITextureHandle CreateHandler(ITexture texture, ITextureCreationSettings settings)
    {
        return new TextureHandle(texture, settings);
    }


    public ITextureHandle CreateHandler(ResourcePath path, ITextureCreationSettings settings)
    {
        return CreateHandler(Create(path), settings);
    }
    
    public ITextureHandle GetHandler(ITexture texture)
    {
        if (_cachedHandles.TryGetValue(texture, out var result))
            return result;
            
        return _cachedHandles[texture] = new TextureHandle(texture, new Texture2DCreationSettings());
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
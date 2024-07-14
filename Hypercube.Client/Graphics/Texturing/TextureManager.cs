using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using StbImageSharp;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureManager : ITextureManager
{
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private readonly Logger _logger = LoggingManager.GetLogger("texturing");

    public TextureManager()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }
    
    public ITextureHandle GetTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        return GetTextureHandleInternal(path, settings);
    }
    
    public ITextureHandle GetTextureHandle(ResourcePath path)
    {
        return GetTextureHandleInternal(path, new Texture2DCreationSettings());
    }
    
    public ITexture GetTexture(ResourcePath path, ITextureCreationSettings settings)
    {
        return GetTextureInternal(path, settings);
    }

    public ITextureHandle GetTextureHandle(ITexture texture)
    {
        return GetTextureHandleInternal(texture.Path, new Texture2DCreationSettings());
    }

    public ITextureHandle GetTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return GetTextureHandleInternal(texture.Path, settings);
    }

    private ITexture GetTextureInternal(ResourcePath path, ITextureCreationSettings settings)
    {
        var texture = CreateTexture(path, settings);
        return texture;
    }

    private ITexture CreateTexture(ResourcePath path, ITextureCreationSettings settings)
    {
        StbImage.stbi_set_flip_vertically_on_load(settings.Flipped ? 0 : 1);
        using var stream = _resourceManager.ReadFileContent(path);
        
        var result = ImageResult.FromStream(stream);
        var texture = new Texture(path, (result.Width, result.Height), result.Data);
        
        return texture;
    }

    private ITextureHandle GetTextureHandleInternal(ResourcePath path, ITextureCreationSettings settings)
    {
        return CreateTextureHandle(path, settings);
    }

    private ITextureHandle CreateTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        var texture = GetTexture(path, settings);
        var handle = new TextureHandle(texture, settings);

        return handle;
    }
    
    private ITextureHandle CreateTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return new TextureHandle(texture, settings);
    }
}
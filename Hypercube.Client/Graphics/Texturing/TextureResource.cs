using Hypercube.Client.Graphics.Texturing.Settings;
using Hypercube.Dependencies;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureResource : Resource, IDisposable
{
    public ResourcePath Path;
    public ITextureHandle Texture = default!;

    public TextureResource()
    {
        Path = string.Empty;
    }
    
    public TextureResource(ResourcePath path)
    {
        Path = path;
    }
    
    protected override void OnLoad(ResourcePath path, DependenciesContainer container)
    {
        var textureManager = container.Resolve<ITextureManager>();
        var handle = textureManager.CreateTextureHandle(path, new TextureCreationSettings());
        Texture = handle;
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}
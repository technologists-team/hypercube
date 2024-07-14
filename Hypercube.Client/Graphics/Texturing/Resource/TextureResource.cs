using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing.Resource;

public sealed class TextureResource : Shared.Resources.Caching.Resource.Resource<TextureMetaData>, IDisposable
{
    public ITextureHandle Texture = default!;
    public ResourcePath Path;

    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        base.Load(path, container);
        
        var textureManager = container.Resolve<ITextureManager>();
        var handle = textureManager.GetTextureHandle(path, MetaData?.Table ?? (ITextureCreationSettings) new Texture2DCreationSettings());
        Texture = handle;
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}
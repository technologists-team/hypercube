using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching.Resource;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Resources.Caching;

public class TextureResource : BaseResource, IDisposable
{
    public ITextureHandle Texture;
    public ResourcePath Path;

    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        var textureManager = container.Resolve<ITextureManager>();
        var handle = textureManager.GetTextureHandle(path, new Texture2DCreationSettings());
        Texture = handle;
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}
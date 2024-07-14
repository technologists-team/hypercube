using Hypercube.Client.Graphics.Texturing.Settings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching.Resource;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureResource : Resource, IDisposable
{
    public ResourcePath Path;
    public ITextureHandle Texture = default!;

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
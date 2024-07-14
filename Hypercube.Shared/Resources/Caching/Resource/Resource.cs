using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Shared.Resources.Caching.Resource;

public abstract class Resource
{
    public ResourceMetaData? MetaData { get; private set; }
    public ResourcePath? FallbackPath { get; }

    public virtual void Load(ResourcePath path, DependenciesContainer container)
    {
        var resourceManager = container.Resolve<IResourceManager>();
        var metaDataPath = $"{path.ParentDirectory}/{path.Filename}.meta";

        MetaData = null;
        if (!resourceManager.TryReadFileContent(metaDataPath, out var stream))
            return;

        var reader = resourceManager.WrapStream(stream);
        LoadMetadata(reader.ReadToEnd());
        
        reader.Dispose();
        stream.Dispose();
    }

    public virtual void Reload(ResourcePath path, DependenciesContainer container)
    {
    }

    protected virtual void LoadMetadata(string toml)
    {
        MetaData = new ResourceMetaData(toml);
    }
}

public abstract class Resource<T> : Resource where T : class, new() 
{
    public new ResourceMetaData<T>? MetaData { get; private set; }

    protected override void LoadMetadata(string toml)
    {
        MetaData = new ResourceMetaData<T>(toml);
    }
}
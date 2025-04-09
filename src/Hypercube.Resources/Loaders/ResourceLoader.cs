using Hypercube.Resources.FileSystems;

namespace Hypercube.Resources.Loaders;

public abstract class ResourceLoader<T> : IResourceLoader where T : Resource
{
    public abstract string[] Extensions { get; }
    
    public Type ResourceType => typeof(T);
        
    public abstract bool CanLoad(ResourcePath path, IFileSystem fileSystem);
    public abstract T Load(ResourcePath path, IFileSystem fileSystem);
        
    Resource IResourceLoader.Load(ResourcePath path, IFileSystem fileSystem)
    {
        return Load(path, fileSystem) ?? throw new NullReferenceException();
    }
}
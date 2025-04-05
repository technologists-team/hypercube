using Hypercube.Resources.FileSystems;

namespace Hypercube.Resources.Loaders;

public interface IResourceLoader
{
    string[] Extensions { get; }
    Type ResourceType { get; }
    
    public bool CanLoad(ResourcePath path, IFileSystem fileSystem);
    public object Load(ResourcePath path, IFileSystem fileSystem);
}
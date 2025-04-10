using Hypercube.Resources.Loaders;
using Hypercube.Resources.Preloading;

namespace Hypercube.Resources;

public interface IResourceManager
{
    void Mount(Dictionary<ResourcePath, ResourcePath> mountFolders);
    void Mount(Dictionary<string, string> mountFolders);
    void Mount(ResourcePath path, ResourcePath physicalPath);
    void Unmount(ResourcePath relativePath);
    void AddLoader<T>(IResourceLoader loader) where T : Resource;
    bool HasLoader<T>() where T : Resource;
    void RemoveLoader<T>() where T : Resource;
    T Get<T>(ResourcePath path) where T : Resource;
    bool HasCache<T>(ResourcePath path) where T : Resource;
    T Load<T>(ResourcePath path) where T : Resource;
    Resource Load(ResourcePath path, Type type);
    Resource Load(ResourcePath path);
    void Unload(ResourcePath path);
    void UnloadAll();
    PreloadContext CreatePreloadContext();
}
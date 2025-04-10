using Hypercube.Resources.FileSystems;
using Hypercube.Resources.Loaders;
using Hypercube.Resources.Preloading;
using JetBrains.Annotations;

namespace Hypercube.Resources;

[UsedImplicitly]
public sealed class ResourceManager : IResourceManager, IDisposable
{
    public readonly IFileSystem FileSystem;
    
    private readonly Dictionary<Type, IResourceLoader> _loaders = [];
    private readonly Dictionary<ResourcePath, Resource> _cache = [];

    public ResourceManager(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }
    
    public ResourceManager()
    {
        FileSystem = new PhysicalFileSystem();
    }
    
    /// <inheritdoc/>
    public void Mount(Dictionary<ResourcePath, ResourcePath> mountFolders)
    {
        FileSystem.Mount(mountFolders);
    }

    /// <inheritdoc/>
    public void Mount(Dictionary<string, string> mountFolders)
    {
        FileSystem.Mount(mountFolders);
    }

    /// <inheritdoc/>
    public void Mount(ResourcePath physicalPath, ResourcePath relativePath)
    {
        FileSystem.Mount(physicalPath, relativePath);
    }

    /// <inheritdoc/>
    public void Unmount(ResourcePath relativePath)
    {
        FileSystem.Unmount(relativePath);
    }

    public void AddLoader<T>(IResourceLoader loader) where T : Resource
    {
        if (_loaders.ContainsKey(typeof(T)))
            throw new Exception();
        
        _loaders.Add(typeof(T), loader);
    }

    public bool HasLoader<T>() where T : Resource
    {
        return _loaders.ContainsKey(typeof(T));
    }

    public void RemoveLoader<T>() where T : Resource
    {
        if (!_loaders.ContainsKey(typeof(T)))
            throw new Exception();

        _loaders.Remove(typeof(T));
    }

    public T Get<T>(ResourcePath path) where T : Resource
    {
        if (_cache.TryGetValue(path, out var resource))
            return resource as T ?? throw new Exception();

        return Load<T>(path);
    }

    public bool HasCache<T>(ResourcePath path) where T : Resource
    {
        return _cache.ContainsKey(path);
    }

    public T Load<T>(ResourcePath path) where T : Resource
    {
        return (T) Load(path, typeof(T));
    }

    public Resource Load(ResourcePath path, Type type)
    {
        if (!_loaders.TryGetValue(type, out var loader))
            throw new Exception();

        if (_cache.TryGetValue(path, out var cache))
            return cache;
        
        var resource = loader.Load(path, FileSystem);
        _cache[path] = resource;
        
        return resource;
    }

    public Resource Load(ResourcePath path)
    {
        if (_cache.TryGetValue(path, out var cache))
            return cache;
        
        foreach (var (_, loader) in _loaders)
        {
            if (loader.Extensions.Contains(path.Extension))
                return loader.Load(path, FileSystem);
        }

        throw new Exception();
    }

    public void Unload(ResourcePath path)
    {
        if (!_cache.Remove(path, out var resource))
            throw new Exception();

        resource.Dispose();
    }
    
    public void UnloadAll()
    {
        foreach (var (path, _) in _cache)
        {
            Unload(path);
        }
    }

    public PreloadContext CreatePreloadContext()
    {
        return new PreloadContext(this);
    }

    public void Dispose()
    {
        UnloadAll();
    }
}
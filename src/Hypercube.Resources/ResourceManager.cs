using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hypercube.Resources.FileSystems;
using Hypercube.Resources.Loaders;
using Hypercube.Resources.Preloading;

namespace Hypercube.Resources;

public class ResourceManager : IResourceManager, IDisposable
{
    private sealed class PathCacheEntry
    {
        public string PhysicalPath { get; set; }
        public DateTime LastCheck { get; set; }
    }
    
    private readonly IFileSystem _fileSystem;
    private readonly ReaderWriterLockSlim _mountsLock = new();
    private Dictionary<string, string> _mountPoints = new();

    private readonly ConcurrentDictionary<string, object> _resources = new();
    private readonly ConcurrentDictionary<string, PathCacheEntry> _pathCache = new();
    private readonly Dictionary<Type, IResourceLoader> _loadersByType;
    private readonly Dictionary<string, List<IResourceLoader>> _loadersByExtension;

    
    public ResourceManager()
    {
        _fileSystem = new PhysicalFileSystem();

        _loadersByType = loaders.ToDictionary(x => x.ResourceType);
        _loadersByExtension = loaders
            .SelectMany(loader => loader.Extensions
                .Select(ext => (ext, loader)))
            .GroupBy(x => x.ext, x => x.loader)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
    
    public T Load<T>(ResourcePath path) where T : class
    {
        var sw = Stopwatch.StartNew();
        
        // 1. Проверка кэша ресурсов (самая частая операция)
        if (_resources.TryGetValue(path, out var cached))
        {
            Debug.WriteLine($"Cache hit: {path} in {sw.ElapsedTicks} ticks");
            return (T) cached;
        }

        // 2. Разрешение физического пути (с кэшированием)
        var physicalPath = ResolvePathCached(path);
        if (physicalPath == null)
            throw new FileNotFoundException($"Resource '{path}' not found");

        // 3. Определение загрузчика
        var extension = Path.GetExtension(physicalPath).ToLowerInvariant();
        if (!_loadersByExtension.TryGetValue(extension, out var candidateLoaders))
            throw new InvalidOperationException($"No loader for extension '{extension}'");

        // 4. Поиск подходящего загрузчика
        foreach (var loader in candidateLoaders)
        {
            if (!loader.CanLoad(path, _fileSystem))
                continue;
            
            var resource = (T) loader.Load(path, _fileSystem);
            _resources[path] = resource;
            
            Debug.WriteLine($"Loaded {path} in {sw.ElapsedMilliseconds}ms");
            return resource;
        }

        throw new InvalidOperationException($"No suitable loader found for {path}");
    }

    private string ResolvePathCached(ResourcePath path)
    {
        if (_pathCache.TryGetValue(path, out var cacheEntry) && 
            (DateTime.UtcNow - cacheEntry.LastCheck).TotalSeconds < 5)
            return cacheEntry.PhysicalPath;
        
        _mountsLock.EnterReadLock();
        
        try
        {
            foreach (var (physicalRoot, virtualRoot) in _mountPoints)
            {
                if (!path.Value.StartsWith(virtualRoot))
                    continue;
                
                var relativePath = path.Value[virtualRoot.Length..];
                var physicalPath = physicalRoot + relativePath;

                if (!_fileSystem.Exists(physicalPath))
                    continue;
                    
                // Update cache
                _pathCache[path] = new PathCacheEntry
                {
                    PhysicalPath = physicalPath,
                    LastCheck = DateTime.UtcNow
                };
                
                return physicalPath;
            }
        }
        finally
        {
            _mountsLock.ExitReadLock();
        }

        return string.Empty;
    }
    
    public bool Unload(ResourcePath path)
    {
        if (!_resources.TryRemove(path, out var resource))
            return false;
        
        if (resource is IDisposable disposable)
            disposable.Dispose();
        
        _pathCache.TryRemove(path, out _);
        return true;
    }

    public void UnloadAll()
    {
        foreach (var resource in _resources.Values.OfType<IDisposable>())
            resource.Dispose();
        
        _resources.Clear();
        _pathCache.Clear();
    }

    public void Dispose()
    {
        UnloadAll();
        _mountsLock.Dispose();
    }
}
using System.Collections.Frozen;

namespace Hypercube.Resources.Preloading;

public class PreloadContext
{
    private readonly ResourceManager _manager;
    private readonly List<ResourcePath> _resources = new();
    
    private FrozenSet<ResourcePath> _frozenResources = FrozenSet<ResourcePath>.Empty;
    
    internal PreloadContext(ResourceManager manager)
    {
        _manager = manager;
    }

    public PreloadContext Add<T>(ResourcePath path) where T : class
    {
        _resources.Add(path.Normalized);
        _frozenResources = _resources.ToFrozenSet();
        
        return this;
    }

    public PreloadContext AddRange<T>(IEnumerable<ResourcePath> paths) where T : class
    {
        _resources.AddRange(paths.Select(p => p.Normalized));
        _frozenResources = _resources.ToFrozenSet();
        
        return this;
    }
    
    public async Task ExecuteAsync(IProgress<PreloadProgress>? progress = null, CancellationToken cancellationToken = default)
    {
        var total = _frozenResources.Count;
        var loaded = 0;
        
        foreach (var path in _frozenResources)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var loader = _manager.FindBestLoader(path);
                if (loader != null)
                {
                    var resolvedPath = _manager.ResolvePath(path);
                    var resource = loader.Load(resolvedPath, _manager._fileSystem);
                    
                    lock (_manager._syncRoot)
                    {
                        var newResources = _manager._resources.ToDictionary();
                        newResources[path] = resource;
                        _manager._resources = newResources.ToFrozenDictionary();
                    }
                }

                loaded++;
                progress?.Report(new PreloadProgress(loaded, total, path));
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                progress?.Report(new PreloadProgress(loaded, total, path, ex));
            }

            await Task.Yield();
        }
    }
}
using Hypercube.Resources.Loaders;
using Hypercube.Utilities.Extensions;

namespace Hypercube.Resources.Preloading;

public sealed class PreloadContext
{
    private readonly ResourceManager _manager;
    private readonly Dictionary<Type, List<ResourcePath>> _resources = [];
    
    internal PreloadContext(ResourceManager manager)
    {
        _manager = manager;
    }

    public PreloadContext Add<T>(ResourcePath path) where T : Resource
    {
        _resources.GetOrInstantiate(typeof(T)).Add(path);
        return this;
    }

    public PreloadContext AddDirectory<T>(ResourcePath path) where T : Resource
    {
        _resources.GetOrInstantiate(typeof(T)).AddRange(_manager.FileSystem.GetFiles(path));
        return this;
    }
    
    public PreloadContext Add<T>(IEnumerable<ResourcePath> paths) where T : Resource
    {
        _resources.GetOrInstantiate(typeof(T)).AddRange(paths);
        return this;
    }
    
    public async Task ExecuteAsync(IProgress<PreloadProgress>? progress = null, CancellationToken cancellationToken = default)
    {
        var total = _resources.Count;
        var loaded = 0;

        foreach (var (type, paths) in _resources)
        {
            foreach (var path in paths)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                
                    _manager.Load(path, type);

                    loaded++;
                    progress?.Report(new PreloadProgress(loaded, total, path));
                }
                catch (Exception ex)
                {
                    progress?.Report(new PreloadProgress(loaded, total, path, ex));
                }

                await Task.Yield();
            }
        }
    }
}
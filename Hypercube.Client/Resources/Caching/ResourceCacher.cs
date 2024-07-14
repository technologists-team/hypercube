using System.Diagnostics.CodeAnalysis;
using Hypercube.Client.Audio.Events;
using Hypercube.Client.Graphics.Events;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching;
using Hypercube.Shared.Resources.Caching.Resource;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Resources.Caching;

public partial class ResourceCacher : IResourceCacher, IEventSubscriber, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private readonly Dictionary<Type, Dictionary<ResourcePath, Resource>> _cachedResources = new();
    private readonly DependenciesContainer _container = DependencyManager.GetContainer();
    private readonly Logger _logger = LoggingManager.GetLogger("cache");
    
    public void PostInject()
    {
        _eventBus.Subscribe<AudioLibraryInitializedEvent>(this, OnAudioLibraryInitialized);
        _eventBus.Subscribe<GraphicsLibraryInitializedEvent>(this, OnGraphicsLibraryInitialized);
    }
    
    public T GetResource<T>(ResourcePath path, bool useFallback = true) where T : Resource, new()
    {
        var typeDict = GetTypeDict<T>();

        if (typeDict.TryGetValue(path, out var cache))
            return (T) cache;

        
        cache = new T();
        try
        {
            cache.Load(path, _container);
            typeDict[path] = cache;
            return (T) cache;
        }
        catch (Exception ex)
        {
            if (useFallback && cache.FallbackPath is not null)
                return GetResource<T>(cache.FallbackPath.Value, false);
            
            _logger.Fatal($"Exception while loading resource {ex.Message}, Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    public bool TryGetResource<T>(ResourcePath path, [NotNullWhen(true)] out T? resource) where T : Resource, new()
    {
        var cont = DependencyManager.GetContainer();
        var cache = GetTypeDict<T>();

        if (cache.TryGetValue(path, out var res))
        {
            resource = (T)res;
            return true;
        }

        res = new T();
        try
        {
            res.Load(path, cont);
            cache[path] = res;
            resource = (T)res;
            return true;
        }
        catch (FileNotFoundException)
        {
            resource = null;
            return false;
        }
        catch (Exception ex)
        {
            _logger.Fatal($"Exception while loading resource: {ex.Message}, Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    public void CacheResource<T>(ResourcePath path, T resource) where T : Resource, new()
    {
        GetTypeDict<T>()[path] = resource;
    }

    private Dictionary<ResourcePath, Resource> GetTypeDict<T>()
    {
        if (_cachedResources.TryGetValue(typeof(T), out var dict)) 
            return dict;
        
        dict = new Dictionary<ResourcePath, Resource>();
        _cachedResources[typeof(T)] = dict;

        return dict;
    }
}
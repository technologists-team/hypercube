using System.Diagnostics.CodeAnalysis;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching;
using Hypercube.Shared.Resources.Caching.Resource;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Resources.Caching;

public partial class CacheManager : ICacheManager, ICacheManagerInternal
{
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private Dictionary<Type, Dictionary<ResourcePath, BaseResource>> _cachedResources =
        new Dictionary<Type, Dictionary<ResourcePath, BaseResource>>();

    private DependenciesContainer _container = default!;

    private readonly Logger _logger = LoggingManager.GetLogger("cache");
    
    #region PublicAPI

    public CacheManager()
    {
        _container = DependencyManager.GetContainer();
    }

    public T GetResource<T>(ResourcePath path, bool useFallback = true) where T : BaseResource, new()
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
            
            _logger.Error($"Exception while loading resource {ex.Message}, Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    public bool TryGetResource<T>(ResourcePath path, [NotNullWhen(true)] out T? resource) where T : BaseResource, new()
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
            _logger.Error($"Exception while loading resource: {ex.Message}, Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    public void CacheResource<T>(ResourcePath path, T resource) where T : BaseResource, new()
    {
        GetTypeDict<T>()[path] = resource;
    }

    public T GetFallback<T>() where T : BaseResource, new()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private

    private Dictionary<ResourcePath, BaseResource> GetTypeDict<T>()
    {
        if (_cachedResources.TryGetValue(typeof(T), out var dict)) 
            return dict;
        
        dict = new Dictionary<ResourcePath, BaseResource>();
        _cachedResources[typeof(T)] = dict;

        return dict;
    }
    
    #endregion
}
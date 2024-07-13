using System.Diagnostics.CodeAnalysis;
using Hypercube.Shared.Resources.Caching.Resource;

namespace Hypercube.Shared.Resources.Caching;

public interface ICacheManager
{
    T GetResource<T>(ResourcePath path, bool useFallback = true) where T : BaseResource, new();
   
    bool TryGetResource<T>(ResourcePath path, [NotNullWhen(true)] out T? resource)
        where T : BaseResource, new();

    void CacheResource<T>(ResourcePath path, T resource)
        where T : BaseResource, new();
}
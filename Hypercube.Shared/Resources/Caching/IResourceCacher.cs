using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Shared.Resources.Caching;

public interface IResourceCacher
{
    T GetResource<T>(ResourcePath path, bool useFallback = true) where T : Resource.Resource, new();
   
    bool TryGetResource<T>(ResourcePath path, [NotNullWhen(true)] out T? resource)
        where T : Resource.Resource, new();

    void CacheResource<T>(ResourcePath path, T resource)
        where T : Resource.Resource, new();
}
using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Resources.Container;

public interface IResourceContainer
{
    T GetResource<T>(ResourcePath path, bool useFallback = true)
        where T : Resource, new();
    bool TryGetResource<T>(ResourcePath path, [NotNullWhen(true)] out T? resource)
        where T : Resource, new();
    bool TryCacheResource<T>(ResourcePath path, T resource)
        where T : Resource, new();
    void CacheResource<T>(ResourcePath path, T resource)
        where T : Resource, new();
    bool Cached<T>(ResourcePath path)
        where T : Resource;
}
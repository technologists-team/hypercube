using Hypercube.Shared.Dependency;

namespace Hypercube.Shared.Resources.Caching.Resource;

public abstract class BaseResource
{
    public abstract void Load(ResourcePath path, DependenciesContainer container);

    public virtual void Reload(ResourcePath path, DependenciesContainer container)
    {
        
    }
    
    public ResourcePath? FallbackPath { get; }
}
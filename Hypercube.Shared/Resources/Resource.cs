using Hypercube.Dependencies;

namespace Hypercube.Shared.Resources;

public abstract class Resource
{
    public virtual ResourcePath? FallbackPath => null;
    public bool HasFallback => FallbackPath is not null;
    public bool Loaded { get; private set; } = false;
    
    public void Load(ResourcePath path, DependenciesContainer container)
    {
        if (Loaded)
            return;
        
        OnLoad(path, container);
        Loaded = true;
    }
    
    protected abstract void OnLoad(ResourcePath path, DependenciesContainer container);

    public virtual void Reload(ResourcePath path, DependenciesContainer container)
    {
    }
}
using Hypercube.Dependencies;
using JetBrains.Annotations;

namespace Hypercube.Resources;

[PublicAPI]
public abstract class Resource
{
    public bool Loaded { get; private set; }

    public bool HasFallback => FallbackPath is not null;

    public virtual ResourcePath? FallbackPath => null;

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
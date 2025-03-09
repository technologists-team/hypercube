using System.Diagnostics.CodeAnalysis;
using Hypercube.Core.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Core.Ecs.Systems;

public abstract class EntitySystem : IEntitySystem
{
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public World World { get; private set; } = default!;

    public virtual void Startup()
    {
    }

    public virtual void Shutdown()
    {
    }

    public virtual void Update(float deltaTime)
    {
    }

    protected bool HasComponent<T>(Entity entity) where T : IComponent
    {
        return World.HasComponent<T>(entity);
    }

    protected bool AddComponent<T>(Entity entity) where T : IComponent
    {
        return World.AddComponent<T>(entity);
    }

    protected bool RemoveComponent<T>(Entity entity) where T : IComponent
    {
        return World.RemoveComponent<T>(entity);
    }

    protected T GetComponent<T>(Entity entity) where T : IComponent
    {
        return World.GetComponent<T>(entity);
    }

    protected T EnsureComponent<T>(Entity entity) where T : IComponent
    {
        return World.EnsureComponent<T>(entity);
    }

    protected bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T? component) where T : IComponent
    {
        return World.TryGetComponent(entity, out component);
    }
}
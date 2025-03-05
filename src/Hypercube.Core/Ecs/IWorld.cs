using System.Diagnostics.CodeAnalysis;
using Hypercube.Core.Ecs.Components;
using Hypercube.Core.Ecs.Systems;

namespace Hypercube.Core.Ecs;

public interface IWorld
{
    void Update(float deltaTime);
    
    bool AddSystem<T>() where T : IEntitySystem;
    IEntitySystem GetSystem<T>() where T : IEntitySystem;

    Entity CreateEntity();
    bool DestroyEntity(Entity entity);
    
    bool AddComponent<T>(Entity entity) where T : IComponent;
    bool RemoveComponent<T>(Entity entity) where T : IComponent;
    bool HasComponent<T>(Entity entity) where T : IComponent;
    IComponent GetComponent<T>(Entity entity) where T : IComponent;
    bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T component) where T : IComponent;
    IComponent EnsureComponent<T>(Entity entity) where T : IComponent;
}
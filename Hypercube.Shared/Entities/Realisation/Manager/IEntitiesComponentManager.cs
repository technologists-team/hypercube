using Hypercube.Shared.Entities.Realisation.Components;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public interface IEntitiesComponentManager
{
    T AddComponent<T>(EntityUid entityUid) where T : IComponent;
    T AddComponent<T>(EntityUid entityUid, Action<Entity<T>> callback) where T : IComponent;
    void RemoveComponent<T>(EntityUid entityUid) where T : IComponent;
    T GetComponent<T>(EntityUid entityUid) where T : IComponent;
    bool HasComponent<T>(EntityUid entityUid) where T : IComponent;
    IEnumerable<Entity<T>> GetEntities<T>() where T : IComponent;
    IEnumerable<Entity<T1, T2>> GetEntities<T1, T2>() where T1 : IComponent where T2 : IComponent;
    IEnumerable<Entity<T1, T2, T3>> GetEntities<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent;
}
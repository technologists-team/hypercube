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
}
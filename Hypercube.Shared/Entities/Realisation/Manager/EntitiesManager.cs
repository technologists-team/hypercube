using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public sealed class EntitiesManager : IEntitiesManager
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly HashSet<EntityUid> _entities = new();
    
    private EntityUid NextEntityUid => new(_nextEntityUid++);
    
    private int _nextEntityUid;

    public EntityUid Create(string name = "New Entity")
    {
        return Create(name, SceneCoordinates.Nullspace);
    }

    public EntityUid Create(string name, SceneCoordinates coordinates)
    {
        var newEntity = NextEntityUid;
        
        _entities.Add(newEntity);
        _eventBus.Raise(new EntityAdded(newEntity, name, coordinates));
        
        return newEntity;
    }
    
    public void Delete(EntityUid entityUid)
    {
        _entities.Remove(entityUid);
        _eventBus.Raise(new EntityRemoved(entityUid));
    }
}
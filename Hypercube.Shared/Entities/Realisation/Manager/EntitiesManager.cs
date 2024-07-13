using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.Entities.Systems.MetaData;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.EventBus;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public sealed class EntitiesManager : IEntitiesManager
{
    [Dependency] private readonly IEntitiesComponentManager _entitiesComponentManager = default!;
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

        var metaDataComponent = _entitiesComponentManager.AddComponent<MetaDataComponent>(newEntity);
        var transformComponent = _entitiesComponentManager.AddComponent<TransformComponent>(newEntity);

        transformComponent.SceneId = coordinates.Scene;
        transformComponent.Transform.SetPosition(coordinates.Position);
        
        _entities.Add(newEntity);
        _eventBus.Raise(new EntityAdded(newEntity));
        
        return newEntity;
    }
    
    public void Delete(EntityUid entityUid)
    {
        _entities.Remove(entityUid);
        _eventBus.Raise(new EntityRemoved(entityUid));
    }
}
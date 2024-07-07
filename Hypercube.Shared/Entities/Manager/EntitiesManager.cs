using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Events;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Timing;

namespace Hypercube.Shared.Entities.Manager;

public sealed class EntitiesManager : IEntitiesManager
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly ITiming _timing = default!;

    private readonly HashSet<EntityUid> _entities = new();
    
    private EntityUid NextEntityUid => new(_nextEntityUid++);
    
    private int _nextEntityUid;

    public EntityUid Create()
    {
        var newEntity = NextEntityUid;

        _entities.Add(newEntity);
        _eventBus.Invoke(new EntityAdded(newEntity));
        return newEntity;
    }

    public void Delete(EntityUid entityUid)
    {
        _entities.Remove(entityUid);
        _eventBus.Invoke(new EntityRemoved(entityUid));
    }
}
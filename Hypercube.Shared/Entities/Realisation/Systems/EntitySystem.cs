using System.Globalization;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.EventBus.Handlers;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Shared.Entities.Realisation.Systems;

public abstract class EntitySystem : IEntitySystem, IEventSubscriber
{
    [Dependency] private readonly IEntitiesComponentManager _entitiesComponentManager = default!;
    [Dependency] private readonly IEntitiesEventBus _entitiesEventBus = default!;
    [Dependency] private readonly IEntitiesManager _entitiesManager = default!;
    [Dependency] private readonly IEntitiesSystemManager _entitiesSystemManager = default!;

    [Dependency] private readonly IEventBus _eventBus = default!;
    
    protected readonly ILogger Logger;

    protected EntitySystem()
    {
        Logger = LoggingManager.GetLogger(GetLoggerName());
    }
    
    public virtual void Initialize()
    {

    }

    public virtual void Shutdown(RuntimeShutdownEvent args)
    {

    }

    public virtual void FrameUpdate(UpdateFrameEvent args)
    {

    }

    protected T AddComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return _entitiesComponentManager.AddComponent<T>(entityUid);
    }
    
    protected T GetComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return _entitiesComponentManager.GetComponent<T>(entityUid);
    }
    
    protected bool HasComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return _entitiesComponentManager.HasComponent<T>(entityUid);
    }

    protected IEnumerable<Entity<T>> GetEntities<T>() where T : IComponent
    {
        return _entitiesComponentManager.GetEntities<T>();
    }

    protected void Subscribe<T>(EventRefHandler<T> callback) where T : IEventArgs
    {
        _eventBus.Subscribe(this, callback);
    }
    
    protected void Unsubscribe<T>(EventRefHandler<T> callback) where T : IEventArgs
    {
        _eventBus.Unsubscribe<T>(this);
    }
    
    private string GetLoggerName()
    {
        var name = GetType().Name;

        // Strip trailing "system"
        if (name.EndsWith("System"))
            name = name.Substring(0, name.Length - "System".Length);

        // Convert CamelCase to snake_case
        // Ignore if all uppercase, assume acronym (e.g. NPC or HTN)
        if (name.All(char.IsUpper))
        {
            name = name.ToLower(CultureInfo.InvariantCulture);
        }
        else
        {
            name = string.Concat(name.Select(x => char.IsUpper(x) ? $"_{char.ToLower(x)}" : x.ToString()));
            name = name.Trim('_');
        }

        return $"system.{name}";
    }
}
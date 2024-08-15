using System.Globalization;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.EventBus.Events;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Shared.Entities.Realisation.Systems;

public abstract class EntitySystem : IEntitySystem, IEventSubscriber
{
    [Dependency] private readonly IEntitiesComponentManager _entitiesComponentManager = default!;
    [Dependency] private readonly IEntitiesEventBus _entitiesEventBus = default!;
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

    protected IEnumerable<Entity<T1, T2>> GetEntities<T1, T2>() where T1 : IComponent where T2 : IComponent
    {
        return _entitiesComponentManager.GetEntities<T1, T2>();
    }

    protected IEnumerable<Entity<T1, T2, T3>> GetEntities<T1, T2, T3>()
        where T1 : IComponent where T2 : IComponent where T3 : IComponent
    {
        return _entitiesComponentManager.GetEntities<T1, T2, T3>();
    }

    protected void Subscribe<T>(EventRefHandler<T> callback) where T : IEventArgs
    {
        _eventBus.Subscribe(this, callback);
    }

    protected void Unsubscribe<T>(EventRefHandler<T> callback) where T : IEventArgs
    {
        _eventBus.Unsubscribe<T>(this);
    }

    protected void Subscribe<TComponent, TEvent>(EntitiesEventRefHandler<TComponent, TEvent> callback)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        _entitiesEventBus.Subscribe(this, callback);
    }

    protected void Raise<TComponent, TEvent>(EntityUid entityUid, TComponent component, TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        _entitiesEventBus.Raise(entityUid, component, args);
    }

    protected void Raise<TComponent, TEvent>(Entity<TComponent> entity, TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        _entitiesEventBus.Raise(entity, args);
    }

    protected void Raise<TComponent, TEvent>(Entity<TComponent> entity, ref TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        _entitiesEventBus.Raise(entity, args);
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
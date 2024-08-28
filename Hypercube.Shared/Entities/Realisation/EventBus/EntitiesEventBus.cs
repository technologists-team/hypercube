using System.Runtime.CompilerServices;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Utilities.Units;

namespace Hypercube.Shared.Entities.Realisation.EventBus;

public sealed class EntitiesEventBus : IEntitiesEventBus
{
    private readonly Dictionary<Type, HashSet<EntitiesEventSubscription>> _eventSubscriptions = new();
    private readonly Dictionary<IEntitySystem, Dictionary<Type, EntitiesEventSubscription>> _systemSubscription = new();

    public void Raise<TEvent>(EntityUid entityUid, IComponent component, TEvent args) where TEvent : IEntitiesEventArgs
    {
        ProcessBroadcastEvent<TEvent>(entityUid, component, ref Unsafe.As<TEvent, Unit>(ref args));
    }

    public void Raise<TComponent, TEvent>(Entity<TComponent> entity, TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        ProcessBroadcastEvent<TEvent>(entity.Owner, entity.Component, ref Unsafe.As<TEvent, Unit>(ref args));
    }

    public void Raise<TComponent, TEvent>(Entity<TComponent> entity, ref TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        ProcessBroadcastEvent<TEvent>(entity.Owner, entity.Component, ref Unsafe.As<TEvent, Unit>(ref args));
    }

    public void Subscribe<TComponent, TEvent>(IEntitySystem subscriber, EntitiesEventRefHandler<TComponent, TEvent> handler)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        Subscribe<TEvent>(subscriber, (EntityUid entityUid, IComponent component, ref Unit unit) =>
        {
            if (component is not TComponent castedComponent)
                return;
            
            ref var tev = ref Unsafe.As<Unit, TEvent>(ref unit);
            handler(new Entity<TComponent>(entityUid, castedComponent), ref tev);
        }, handler);
    }

    public void Unsubscribe<TComponent, TEvent>(IEntitySystem subscriber)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs
    {
        if (!_systemSubscription.TryGetValue(subscriber, out var systemSubscriptions))
            throw new InvalidOperationException();

        if (!systemSubscriptions.ContainsKey(typeof(TEvent)))
            throw new InvalidOperationException();

        systemSubscriptions.Remove(typeof(TEvent));
    }

    private void Subscribe<TEvent>(IEntitySystem subscriber, EntitiesEventRefHandler handler, object equality)
        where TEvent : IEntitiesEventArgs
    {
        if (!_eventSubscriptions.TryGetValue(typeof(TEvent), out var eventSubscriptions))
        {
            eventSubscriptions = new HashSet<EntitiesEventSubscription>();
            _eventSubscriptions[typeof(TEvent)] = eventSubscriptions;
        }

        if (!_systemSubscription.TryGetValue(subscriber, out var systemSubscriptions))
        {
            systemSubscriptions = new Dictionary<Type, EntitiesEventSubscription>();
            _systemSubscription[subscriber] = systemSubscriptions;
        }
        
        var subscription = new EntitiesEventSubscription(handler, equality);

        eventSubscriptions.Add(subscription);
        systemSubscriptions.Add(typeof(TEvent), subscription);
    }

    private void ProcessBroadcastEvent<TEvent>(EntityUid entityUid, IComponent component, ref Unit unit)
        where TEvent : IEntitiesEventArgs
    {
        if (!_eventSubscriptions.TryGetValue(typeof(TEvent), out var registration))
            return;

        foreach (var handler in registration)
        {
            handler.Handler(entityUid, component, ref unit);
        }
    }
}
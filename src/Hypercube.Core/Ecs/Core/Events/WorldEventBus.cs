using System.Runtime.CompilerServices;
using Hypercube.Utilities.Extensions;
using Hypercube.Utilities.References;

namespace Hypercube.Core.Ecs.Core.Events;

[EngineInternal]
public class WorldEventBus
{
    private readonly Dictionary<Type, List<EventSubscription>> _eventRegistration = new();
    private readonly Dictionary<Type, List<GlobalEventSubscription>> _globalEventRegistration = new();
    private readonly Dictionary<Type, List<ComponentEventSubscription>> _componentEventRegistration = new();

     #region Global Events
    public void Raise<TEvent>(ref TEvent ev)
        where TEvent : IEvent
    {
        ProcessGlobalEvent<TEvent>(ref Unsafe.As<TEvent, Unit>(ref ev));
    }

    public void Subscribe<TEvent>(GlobalEventRefHandler<TEvent> handler)
        where TEvent : IEvent
    {
        SubscribeGlobalEventCommon<TEvent>((ref Unit unit) =>
        {
            ref var tev = ref Unsafe.As<Unit, TEvent>(ref unit);
            handler(ref tev);
        }, handler);
    }

    private void SubscribeGlobalEventCommon<TEvent>(GlobalEventRefHandler refHandler, object equality)
        where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        var subscription = new GlobalEventSubscription(refHandler, equality);

        _globalEventRegistration.GetOrInstantiate(eventType).Add(subscription);
    }

    private void ProcessGlobalEvent<TEvent>(ref Unit unit)
        where TEvent : IEvent
    {
        if (!_globalEventRegistration.TryGetValue(typeof(TEvent), out var eventSubscriptions))
            return;

        foreach (var subscription in eventSubscriptions)
            subscription.Handler(ref unit);
    }
    #endregion

    #region Component Events
    public void Raise<TComp, TEvent>(TComp component, ref TEvent ev)
        where TComp : IComponent where TEvent : IEvent
    {
        ProcessComponentEvent<TComp, TEvent>(ref component, ref Unsafe.As<TEvent, Unit>(ref ev));
    }

    public void Subscribe<TComp, TEvent>(ComponentEventRefHandler<TComp, TEvent> handler)
        where TComp : IComponent where TEvent : IEvent
    {
        SubscribeComponentEventCommon<TEvent>((ref IComponent component, ref Unit unit) =>
        {
            if (component is not TComp castedComponent)
                return;
            
            ref var tev = ref Unsafe.As<Unit, TEvent>(ref unit);
            handler(ref castedComponent, ref tev);
        }, handler);
    }

    private void SubscribeComponentEventCommon<TEvent>(ComponentEventRefHandler refHandler, object equality)
        where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        var subscription = new ComponentEventSubscription(refHandler, equality);

        _componentEventRegistration.GetOrInstantiate(eventType).Add(subscription);
    }

    private void ProcessComponentEvent<TComp, TEvent>(ref TComp component, ref Unit unit)
        where TComp : IComponent where TEvent : IEvent
    {
        if (!_componentEventRegistration.TryGetValue(typeof(TEvent), out var eventSubscriptions))
            return;

        if (component is not IComponent castedComponent)
            throw new InvalidOperationException();

        foreach (var subscription in eventSubscriptions)
            subscription.Handler(ref castedComponent, ref unit);
    }
    #endregion

    #region Directed Events (Entity + Component)
    public void Raise<TComp, TEvent>(Entity entity, TComp component, ref TEvent ev)
        where TComp : IComponent where TEvent : IEvent
    {
        ProcessDirectedEvent<TComp, TEvent>(ref entity, ref component, ref Unsafe.As<TEvent, Unit>(ref ev));
    }

    public void Subscribe<TComp, TEvent>(EventRefHandler<TComp, TEvent> handler)
        where TComp : IComponent where TEvent : IEvent
    {
        SubscribeEventCommon<TEvent>((ref Entity entity, ref IComponent component, ref Unit unit) =>
        {
            if (component is not TComp castedComponent)
                return;
            
            ref var tev = ref Unsafe.As<Unit, TEvent>(ref unit);
            handler(ref entity, ref castedComponent, ref tev);
        }, handler);
    }

    private void SubscribeEventCommon<TEvent>(EventRefHandler refHandler, object equality)
        where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        var subscription = new EventSubscription(refHandler, equality);

        _eventRegistration.GetOrInstantiate(eventType).Add(subscription);
    }
    
    private void ProcessDirectedEvent<TComp, TEvent>(ref Entity entity, ref TComp component, ref Unit unit)
        where TComp : IComponent where TEvent : IEvent
    {
        if (!_eventRegistration.TryGetValue(typeof(TEvent), out var eventSubscriptions))
            return;

        if (component is not IComponent castedComponent)
            throw new InvalidOperationException();

        foreach (var subscription in eventSubscriptions)
            subscription.Handler(ref entity, ref castedComponent, ref unit);
    }
    #endregion
}
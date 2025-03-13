using System.Runtime.CompilerServices;
using Hypercube.Core.Analyzers;
using Hypercube.Utilities.Extensions;
using Hypercube.Utilities.References;

namespace Hypercube.Core.Ecs.Core.Events;

[EngineCore]
public class WorldEventBus
{
    private readonly Dictionary<Type, List<EventSubscription>> _eventRegistration = new();

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
            throw new InvalidOperationException();

        if (component is not IComponent castedComponent)
            throw new InvalidOperationException();

        foreach (var subscription in eventSubscriptions)
            subscription.Handler(ref entity, ref castedComponent, ref unit);
    }
}
using System.Runtime.CompilerServices;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.Utilities.Ref;
using Hypercube.Shared.Utilities.Units;

namespace Hypercube.Shared.EventBus;

public sealed class EventBus : IEventBus
{
    private readonly Dictionary<Type, HashSet<EventSubscription>> _eventRegistration = new();
    private readonly Dictionary<IEventSubscriber, Dictionary<Type, EventSubscription>> _subscriptionRegistrations = new();
    
    public void Raise<T>(ref T eventArgs) where T : IEventArgs
    {
        ProcessBroadcastEvent<T>(ref Unsafe.As<T, Unit>(ref eventArgs));
    }

    public void Raise<T>(IEventSubscriber subscriber, ref T eventArgs) where T : IEventArgs
    {
        ProcessDirectedEvent<T>(subscriber, ref Unsafe.As<T, Unit>(ref eventArgs));
    }
    
    public void Raise<T>(T eventArgs) where T : IEventArgs
    {
        ProcessBroadcastEvent<T>(ref Unsafe.As<T, Unit>(ref eventArgs));
    }
    
    public void Raise<T>(IEventSubscriber subscriber, T eventArgs) where T : IEventArgs
    {
        ProcessDirectedEvent<T>(subscriber, ref Unsafe.As<T, Unit>(ref eventArgs));
    }
    
    public void Subscribe<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : IEventArgs
    {
        SubscribeEventCommon<T>(subscriber, (ref Unit ev) =>
        {
            ref var tev = ref Unsafe.As<Unit, T>(ref ev);
            refHandler(ref tev);
        }, refHandler);
    }
    
    /// <exception cref="ArgumentNullException">Throws when subscriber is null</exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void SubscribeEventCommon<T>(IEventSubscriber subscriber, RefHandler refHandler, object equality)
        where T : IEventArgs
    {
        var eventType = typeof(T);
        var subscription = new EventSubscription(refHandler, equality);

        if (!_eventRegistration.TryGetValue(eventType, out var eventRegistration))
        {
            eventRegistration = new HashSet<EventSubscription>();
            _eventRegistration[eventType] = eventRegistration;
        }

        if (!_subscriptionRegistrations.TryGetValue(subscriber, out var subscriptionRegistration))
        {
            subscriptionRegistration = new Dictionary<Type, EventSubscription>();
            _subscriptionRegistrations[subscriber] = subscriptionRegistration;
        }
        
        eventRegistration.Add(subscription);
        subscriptionRegistration.Add(typeof(T), subscription);
    }
    
    public void Unsubscribe<T>(IEventSubscriber subscriber) where T : IEventArgs
    {
        if (!_subscriptionRegistrations.TryGetValue(subscriber, out var subscriptionRegistration))
            throw new InvalidOperationException();
        
        if (!_eventRegistration.TryGetValue(typeof(T), out var eventRegistration))
            throw new InvalidOperationException();

        if (!subscriptionRegistration.TryGetValue(typeof(T), out var eventSubscription))
            throw new InvalidOperationException();
        
        eventRegistration.Remove(eventSubscription);
        subscriptionRegistration.Remove(typeof(T));
    }

    private void ProcessBroadcastEvent<T>(ref Unit unit) where T : IEventArgs
    {
        ProcessBroadcastEvent(ref unit, typeof(T));
    }
    
    private void ProcessBroadcastEvent(ref Unit unit, Type eventType)
    {
        if (!_eventRegistration.TryGetValue(eventType, out var registration))
            return;
        
        foreach (var handler in registration)
        {
            handler.Handler(ref unit);
        }
    }

    private void ProcessDirectedEvent<T>(IEventSubscriber target, ref Unit unit) where T : IEventArgs
    {
        if (!_subscriptionRegistrations.TryGetValue(target, out var subscriptionRegistration))
            throw new InvalidOperationException();

        if (!subscriptionRegistration.TryGetValue(typeof(T), out var eventSubscription))
            throw new InvalidOperationException();

        eventSubscription.Handler(ref unit);
    }
}
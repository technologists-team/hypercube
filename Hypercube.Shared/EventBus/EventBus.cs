using System.Runtime.CompilerServices;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.EventBus.Events.Broadcast;
using Hypercube.Shared.EventBus.Events.Events;
using Hypercube.Shared.EventBus.Events.Exceptions;
using Hypercube.Shared.EventBus.Events.Handlers;

namespace Hypercube.Shared.EventBus;

public sealed class EventBus : IEventBus
{
    private readonly Dictionary<Type, EventRegistration> _eventRegistration = new();
    private readonly Dictionary<IEventSubscriber, Dictionary<Type, BroadcastRegistration>> _inverseEventSubscriptions = new();
    
    public void Raise<T>(ref T receiver) where T : IEventArgs
    {
        ProcessEvent(ref Unsafe.As<T, Unit>(ref receiver), typeof(T));
    }
    
    public void Raise<T>(T receiver) where T : IEventArgs
    {
        ProcessEvent(ref Unsafe.As<T, Unit>(ref receiver), typeof(T));
    }
    
    public void Raise(IEventArgs eventArgs)
    {
        var eventType = eventArgs.GetType();
        ref var unitRef = ref ExtractUnitRef(ref eventArgs, eventType);
        
        ProcessEvent(ref unitRef, eventType);
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
        ArgumentNullException.ThrowIfNull(subscriber);
        
        var eventType = typeof(T);
        var subscription = new BroadcastRegistration(refHandler, equality);
        
        var subscriptions = GetEventRegistration(eventType);
        subscriptions.Add(subscription);
        
        var inverseSubscriptions = GetEventInverseSubscription(subscriber);
        if (inverseSubscriptions.TryAdd(eventType, subscription))
            return;
        
        throw new InvalidOperationException();
    }

    /// <exception cref="ArgumentNullException">Throws when subscriber is null</exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Unsubscribe<T>(IEventSubscriber subscriber) where T : IEventArgs
    {
        ArgumentNullException.ThrowIfNull(subscriber);
        
        var eventType = typeof(T);
        
        var inverseSubscriptions = GetEventInverseSubscription(subscriber);
        if (!inverseSubscriptions.TryGetValue(eventType, out var registration))
            throw new InvalidOperationException();
        
        Unsubscribe(eventType, registration, subscriber);
    }

    /// <exception cref="UnregisteredEventException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void Unsubscribe(Type eventType, BroadcastRegistration registration, IEventSubscriber subscriber)
    {
        var eventRegistration = GetEventRegistration(eventType, false);
        eventRegistration.Remove(registration);
        
        var inverseSubscriptions = GetEventInverseSubscription(subscriber, false);
        inverseSubscriptions.Remove(eventType);
    }
    
    private void ProcessEvent(ref Unit unitRef, Type eventType)
    {
        if (!_eventRegistration.TryGetValue(eventType, out var registration))
            return;
        
        ProcessEventCore(ref unitRef, registration);
    }
    
    private void ProcessEventCore(ref Unit unitRef, EventRegistration registration)
    {
        foreach (var handler in registration.BroadcastRegistrations)
        {
            handler.Handler(ref unitRef);
        }
    }
    
    /// <param name="eventType">Type of event whose registration we want to receive.</param>
    /// <param name="autoRegistration">Allows you to control the automatic registration of an event if it does not exist.</param>
    /// <exception cref="UnregisteredEventException">If <c>autoRegistration</c> is <c>false</c>, it will throw an exception if registration is not found.</exception>
    private EventRegistration GetEventRegistration(Type eventType, bool autoRegistration = true)
    {
        if (_eventRegistration.TryGetValue(eventType, out var found))
            return found;

        if (!autoRegistration)
            throw new UnregisteredEventException(eventType);
        
        return _eventRegistration[eventType] = new EventRegistration();
    }

    /// <exception cref="InvalidOperationException"></exception>
    private Dictionary<Type, BroadcastRegistration> GetEventInverseSubscription(IEventSubscriber subscriber, bool creating = true)
    {
        if (_inverseEventSubscriptions.TryGetValue(subscriber, out var subscriptions))
            return subscriptions;

        if (!creating)
            throw new InvalidOperationException();
        
        return _inverseEventSubscriptions[subscriber] = new Dictionary<Type, BroadcastRegistration>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref Unit ExtractUnitRef(ref IEventArgs eventArgs, Type objType)
    {
        //return ref Unsafe.As<IEventArgs, Unit>(ref eventArgs);
        
        // Why not only unit?
        return ref objType.IsValueType
            ? ref Unsafe.As<IEventArgs, UnitBox>(ref eventArgs).Value
            : ref Unsafe.As<IEventArgs, Unit>(ref eventArgs);
    }
}
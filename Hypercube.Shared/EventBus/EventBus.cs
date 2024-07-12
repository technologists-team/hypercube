using System.Runtime.CompilerServices;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.EventBus.Events.Broadcast;

namespace Hypercube.Shared.EventBus;

public sealed class EventBus : IEventBus
{
    private readonly Dictionary<Type, EventData> _eventData = new();
    private readonly Dictionary<IEventSubscriber, Dictionary<Type, BroadcastRegistration>> _inverseEventSubscriptions = new();
    
    public void Subscribe<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : IEventArgs
    {
        SubscribeEventCommon<T>(subscriber, (ref Unit ev) =>
        {
            ref var tev = ref Unsafe.As<Unit, T>(ref ev);
            refHandler(ref tev);
        }, refHandler);
    }
    
    private void SubscribeEventCommon<T>(IEventSubscriber subscriber, RefHandler refHandler, object equality)
        where T : IEventArgs
    {
        ArgumentNullException.ThrowIfNull(subscriber);
        
        var eventType = typeof(T);
        var subscription = new BroadcastRegistration(refHandler, equality);
        
        var subscriptions = GetEventData(eventType);
        if (subscriptions.BroadcastRegistrations.Contains(subscription))
            throw new InvalidOperationException();
            
        subscriptions.BroadcastRegistrations.Add(subscription);

        Dictionary<Type, BroadcastRegistration>? inverseSubs;
        if (!_inverseEventSubscriptions.TryGetValue(subscriber, out inverseSubs))
        {
            inverseSubs = new Dictionary<Type, BroadcastRegistration>();
            _inverseEventSubscriptions[subscriber] = inverseSubs;
        }
        
        if (!inverseSubs.TryAdd(eventType, subscription))
            throw new InvalidOperationException();
    }

    private EventData GetEventData(Type eventType)
    {
        if (_eventData.TryGetValue(eventType, out var found))
            return found;
        
        var list = new List<BroadcastRegistration>();
        var data = new EventData(list);

        return _eventData[eventType] = data;
    }

    public void Unsubscribe<T>(IEventSubscriber subscriber) where T : IEventArgs
    {
        ArgumentNullException.ThrowIfNull(subscriber);
        var eventType = typeof(T);
        
        if (_inverseEventSubscriptions.TryGetValue(subscriber, out var inverse)
            && inverse.TryGetValue(eventType, out var tuple))
            UnsubscribeEvent(eventType, tuple, subscriber);
    }

    private void UnsubscribeEvent(Type evType, BroadcastRegistration tuple, IEventSubscriber subscriber)
    {
        if (_eventData.TryGetValue(evType, out var subs) &&
            subs.BroadcastRegistrations.Contains(tuple))
        {
            subs.BroadcastRegistrations.Remove(tuple);
        }
        
        if (_inverseEventSubscriptions.TryGetValue(subscriber, out var inverse) && inverse.ContainsKey(evType))
            inverse.Remove(evType);
    }

    public void Raise(object receiver)
    {
        ArgumentNullException.ThrowIfNull(receiver);

        var evType = receiver.GetType();
        ref var unitRef = ref ExtractUnitRef(ref receiver, evType);
        
        ProcessEvent(ref unitRef, evType);
    }
    public void Raise<T>(ref T receiver) where T : IEventArgs
    {
        ProcessEvent(ref Unsafe.As<T, Unit>(ref receiver), typeof(T));
    }
    
    public void Raise<T>(T receiver) where T : IEventArgs
    {
        ProcessEvent(ref Unsafe.As<T, Unit>(ref receiver), typeof(T));
    }
    
    private void ProcessEvent(ref Unit unitRef, Type evType)
    {
        if (!_eventData!.TryGetValue(evType, out var data))
            return;
        
        ProcessEventCore(ref unitRef, data);
    }
    
    private void ProcessEventCore(ref Unit unitRef, EventData data)
    {
        foreach (var handler in data.BroadcastRegistrations)
        {
            handler.Handler(ref unitRef);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref Unit ExtractUnitRef(ref object obj, Type objType)
    {
        return ref objType.IsValueType
            ? ref Unsafe.As<object, UnitBox>(ref obj).Value
            : ref Unsafe.As<object, Unit>(ref obj);
    }
}
using System.Runtime.CompilerServices;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.EventBus.Events.Broadcast;
using EventArgs = Hypercube.Shared.EventBus.Events.EventArgs;

namespace Hypercube.Shared.EventBus;

// TODO: BURNING IN HELL SHIT!!!! Rewrite it
public class EventBus : IEventBus
{
    private readonly Dictionary<Type, EventData> _eventData = new();
    private readonly Dictionary<IEventSubscriber, Dictionary<Type, BroadcastRegistration>> _inverseEventSubscriptions
        = new();


    public void SubscribeEvent<T>(
        IEventSubscriber subscriber, 
        EventRefHandler<T> refHandler) where T : notnull
    {
        SubscribeEventCommon<T>(subscriber, ((ref Unit ev) =>
        {
            ref var tev = ref Unsafe.As<Unit, T>(ref ev);
            refHandler(ref tev);
        }), refHandler);
    }
    
    private void SubscribeEventCommon<T>(
        IEventSubscriber subscriber, 
        RefHandler refHandler, 
        object equality) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(subscriber);
        var evType = typeof(T);
        
        var subscription = new BroadcastRegistration(refHandler, equality);
        
        RegisterCommon(evType, out var subs);
        if (!subs.BroadcastRegistrations.Contains(subscription))
            subs.BroadcastRegistrations.Add(subscription);

        Dictionary<Type, BroadcastRegistration>? inverseSubs;
        if (!_inverseEventSubscriptions.TryGetValue(subscriber, out inverseSubs))
        {
            inverseSubs = new Dictionary<Type, BroadcastRegistration>();
            _inverseEventSubscriptions[subscriber] = inverseSubs;
        }
        
        if (!inverseSubs.TryAdd(evType, subscription))
            throw new InvalidOperationException();
    }

    private void RegisterCommon(Type evType, out EventData data)
    {
        if (!_eventData.TryGetValue(evType, out var found))
        {
            var list = new List<BroadcastRegistration>();
            data = new EventData(list);
            _eventData[evType] = data;
            return;
        }
        data = found;
    }

    private void UnsubscribeEvent<T>(IEventSubscriber subscriber)
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

    public void RaiseEvent(object toRaise)
    {
        ArgumentNullException.ThrowIfNull(toRaise);

        var evType = toRaise.GetType();
        ref var unitRef = ref ExtractUnitRef(ref toRaise, evType);
        
        ProcessEvent(ref unitRef, evType);
    }
    public void RaiseEvent<T>(ref T toRaise) where T : notnull
    {
        ProcessEvent(ref Unsafe.As<T, Unit>(ref toRaise), typeof(T));
    }
    
    public void RaiseEvent<T>(T toRaise) where T : notnull
    {
        ProcessEvent(ref Unsafe.As<T, Unit>(ref toRaise), typeof(T));
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
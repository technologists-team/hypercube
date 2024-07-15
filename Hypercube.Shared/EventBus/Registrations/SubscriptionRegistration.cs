using Hypercube.Shared.EventBus.Broadcast;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.EventBus.Registrations;

/// <summary>
/// Stores information about all events a particular listener is subscribed to.
/// </summary>
public readonly struct SubscriptionRegistration
{
    private readonly WeakReference<IEventSubscriber> _subscriber;
    private readonly Type _subscriberType;
    private readonly Dictionary<Type, EventSubscription> _subscriptions = new();
    
    public SubscriptionRegistration(IEventSubscriber subscriber)
    {
        _subscriber = new WeakReference<IEventSubscriber>(subscriber);
        _subscriberType = subscriber.GetType();
    }

    public void Add<T>(EventSubscription subscription) where T : IEventArgs
    {
        Add(typeof(T), subscription);
    }

    public void Remove<T>() where T : IEventArgs
    {
        Remove(typeof(T));
    }
    
    public EventSubscription Get<T>() where T : IEventArgs
    {
        return Get(typeof(T));
    }
    
    private void Add(Type type, EventSubscription subscription)
    {
        if (_subscriptions.ContainsKey(type))
            throw new InvalidOperationException($"{_subscriberType.Name} unable subscribe to already subscribed event {type.Name}");
        
        _subscriptions.Add(type, subscription);
    }

    private void Remove(Type type)
    {
        if (!_subscriptions.ContainsKey(type))
            throw new InvalidOperationException($"{_subscriberType.Name} unable unsubscribe from an not subscribed event {type.Name}");
        
        _subscriptions.Remove(type);
    }
    
    private EventSubscription Get(Type type)
    {
        if (!_subscriptions.TryGetValue(type, out var subscription))
            throw new InvalidOperationException($"{_subscriberType.Name} unable get subscription from an not subscribed event {type.Name}");

        return subscription;
    }
}
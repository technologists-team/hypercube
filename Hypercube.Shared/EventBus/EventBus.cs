namespace Hypercube.Shared.EventBus;

// TODO: BURNING IN HELL SHIT!!!! Rewrite it
public class EventBus : IEventBus
{
    private readonly Dictionary<Type, HashSet<Delegate>> _callbacks = new();
    
    public void Subscribe<T>(Action<T> callback)
    {
        GetListeners<T>().Add(callback);
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        GetListeners<T>().Remove(callback);
    }

    public void Invoke<T>(T signal)
    {
        foreach (var reference in GetListeners<T>())
        {
            if (reference is not Action<T> action)
                continue;
            
            action.Invoke(signal);
        }
    }

    private HashSet<Delegate> GetListeners<T>()
    {
        if (_callbacks.TryGetValue(typeof(T), out var listeners))
            return listeners;

        return _callbacks[typeof(T)] = new HashSet<Delegate>();
    }
}
namespace Hypercube.Shared.EventBus;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, HashSet<WeakReference<Delegate>>> _callbacks = new();
    
    public void Subscribe<T>(Action<T> callback)
    {
        GetListeners<T>().Add(new WeakReference<Delegate>(callback));
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        GetListeners<T>().Remove(new WeakReference<Delegate>(callback));
    }

    public void Invoke<T>(T signal)
    {
        foreach (var reference in GetListeners<T>())
        {
            if (!reference.TryGetTarget(out var @delegate))
                continue;
            
            if (@delegate is not Action<T> action)
                continue;
            
            action.Invoke(signal);
        }
    }

    private HashSet<WeakReference<Delegate>> GetListeners<T>()
    {
        if (_callbacks.TryGetValue(typeof(T), out var listeners))
            return listeners;

        return _callbacks[typeof(T)] = new HashSet<WeakReference<Delegate>>();
    }
}
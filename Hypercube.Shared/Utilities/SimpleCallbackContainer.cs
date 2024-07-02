namespace Hypercube.Shared.Utilities;

public sealed class SimpleCallbackContainer<TBaseArgs>  where TBaseArgs : notnull
{
    private readonly Dictionary<Type, Action<TBaseArgs>> _callbacks = new();

    public void Register<TArgs>(Action<TArgs> callback)
    {
        if (_callbacks.ContainsKey(typeof(TArgs)))
            throw new Exception();
        
        _callbacks.Add(typeof(TArgs), args =>
        {
            var obj = Convert.ChangeType(args, typeof(TArgs));
            if (obj is not TArgs casted)
                return;
            
            callback.Invoke(casted);
        });
    }

    public void Invoke(TBaseArgs args)
    {
        if (!_callbacks.TryGetValue(args.GetType(), out var callback))
            return;
        
        callback.Invoke(args);
    }
}
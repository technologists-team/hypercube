using System.Diagnostics;

namespace Hypercube.Shared.Dependency;

/// <summary>
/// 
/// </summary>
public static class DependencyManager
{
    private static readonly ThreadLocal<DependenciesContainer> _container = new();
    
    public static void InitThread(DependenciesContainer collection, bool replaceExisting = false)
    {
        if (_container.IsValueCreated && !replaceExisting)
            throw new InvalidOperationException();
        
        _container.Value = collection;
    }
    
    public static DependenciesContainer InitThread()
    {
        if (_container.IsValueCreated)
            return _container.Value!;
        
        var deps = new DependenciesContainer();
        _container.Value = deps;
        
        return deps;
    }

    public static void Register<TType, TImplementation>()
    {
        Debug.Assert(_container.IsValueCreated);
        _container.Value!.Register<TType, TImplementation>();
    }
    
    public static void Register<T>()
    {
        Debug.Assert(_container.IsValueCreated);
        _container.Value!.Register<T>();
    }
    
    public static void Register<T>(T instance)
    {
        Debug.Assert(_container.IsValueCreated);
        _container.Value!.Register(instance);
    }
    
    public static void Register<T>(Func<DependenciesContainer, T> factory)
    {
        Debug.Assert(_container.IsValueCreated);
        _container.Value!.Register(factory);
    }
    
    public static T Resolve<T>()
    {
        Debug.Assert(_container.IsValueCreated);
        return _container.Value!.Resolve<T>();
    }

    public static void Inject(object instance)
    {
        Debug.Assert(_container.IsValueCreated);
        _container.Value!.Inject(instance);
    }

    public static void Clear()
    {
        Debug.Assert(_container.IsValueCreated);
        _container.Value!.Clear();
    }

    public static DependenciesContainer Create()
    {
        Debug.Assert(_container.IsValueCreated);
        return new DependenciesContainer(_container.Value!);
    }
}
using System.Diagnostics;
using JetBrains.Annotations;

namespace Hypercube.Dependencies;

/// <summary>
/// Implementing the IoC mechanism through dependency injection.
/// </summary>
/// <remarks>
/// <para>
/// If you register a dependency in this container with a type that has already been declared in the parent container
/// from another implementation, this will override it, and when you try to resolve the dependency,
/// the value from this container will be returned to you.
/// </para>
/// </remarks>
/// <seealso cref="DependencyAttribute"/>
[PublicAPI]
public static class DependencyManager
{
    private static readonly ThreadLocal<DependenciesContainer> Container = new();
    
    public static void InitThread(DependenciesContainer collection, bool replaceExisting = false)
    {
        if (Container.IsValueCreated && !replaceExisting)
            throw new InvalidOperationException();
        
        Container.Value = collection;
    }
    
    public static DependenciesContainer InitThread()
    {
        if (Container.IsValueCreated)
            return Container.Value!;
        
        var deps = new DependenciesContainer();
        Container.Value = deps;
        
        return deps;
    }

    public static void Register<TType, TImplementation>()
    {
        Debug.Assert(Container.IsValueCreated);
        Container.Value!.Register<TType, TImplementation>();
    }
    
    public static void Register<T>()
    {
        Debug.Assert(Container.IsValueCreated);
        Container.Value!.Register<T>();
    }
    
    public static void Register<T>(T instance)
    {
        Debug.Assert(Container.IsValueCreated);
        Container.Value!.Register(instance);
    }
    
    public static void Register<T>(Func<DependenciesContainer, T> factory)
    {
        Debug.Assert(Container.IsValueCreated);
        Container.Value!.Register(factory);
    }
    
    public static T Resolve<T>()
    {
        Debug.Assert(Container.IsValueCreated);
        return Container.Value!.Resolve<T>();
    }

    public static void Inject(object instance)
    {
        Debug.Assert(Container.IsValueCreated);
        Container.Value!.Inject(instance);
    }

    public static void Clear()
    {
        Debug.Assert(Container.IsValueCreated);
        Container.Value!.Clear();
    }

    public static DependenciesContainer Create()
    {
        Debug.Assert(Container.IsValueCreated);
        return new DependenciesContainer(Container.Value!);
    }

    public static DependenciesContainer GetContainer()
    {
        Debug.Assert(Container.IsValueCreated);
        return Container.Value!;
    }
}
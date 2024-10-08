﻿using System.Reflection;
using Hypercube.Logging;
using Hypercube.Utilities.Extensions;
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
public sealed class DependenciesContainer
{
    private readonly DependenciesContainer? _parent;

    private readonly Dictionary<Type, Func<DependenciesContainer, object>> _factories = new();
    private readonly Dictionary<Type, object> _instances = new();
    private readonly HashSet<Type> _resolutions = [];

    private readonly object _lock = new();

#if DEBUG
    private readonly Logger _debugLogger = LoggingManager.GetLogger("di_container");
#endif
    
    public DependenciesContainer(DependenciesContainer? parent = null)
    {
        _parent = parent;
    }
    
    public void Register<T>()
    {
        Register(typeof(T));
    }

    public void Register(Type type)
    {
        Register(type, type);
    }

    public void Register<TType, TImpl>()
    {
        Register(typeof(TType), typeof(TImpl));
    }

    public void Register(Type type, Type impl)
    {
        object DefaultFactory(DependenciesContainer container)
        {
            var constructors = impl.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (constructors.Length != 1)
                throw new InvalidOperationException();

            var constructor = constructors[0];
            
            var constructorParams = constructor.GetParameters();
            if (constructorParams.Length != 0)
                throw new InvalidOperationException();

            return constructor.Invoke([]);
        }   
        
        Register(type, DefaultFactory);
    }
    
    public void Register<T>([System.Diagnostics.CodeAnalysis.NotNull] T instance)
    {
        if (instance is null)
            throw new ArgumentNullException();
        
        Register<T>(_ => instance);
    }

    public void Register(Type type, object instance)
    {
        Register(type, _ => instance);
    }
    
    public void Register<T>(Func<DependenciesContainer, T> factory)
    {
       Register(typeof(T), container => factory.Invoke(container) ?? throw new InvalidOperationException());
    }

    public void Register(Type type, Func<DependenciesContainer, object> factory)
    {
        lock (_lock)
        {
            _factories[type] = factory;
        }
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    public void Inject(object instance)
    {
        // Encapsulate system method arguments
        Inject(instance, false);
    }

    public void InstantiateAll()
    {
        lock (_lock)
        {
            foreach (var (type, _) in _factories)
            {
                if (_instances.ContainsKey(type))
                    continue;
                
                Instantiate(type);
            }
        }
    }
    
    public object Instantiate<T>()
    {
        return Instantiate(typeof(T));
    }
    
    public object Instantiate(Type type)
    {
        lock (_lock)
        {
            var instance = _factories[type].Invoke(this);
            _instances[type] = instance;
            Inject(instance, true);
            return instance;
        }
    }
    
    private void Inject(object instance, bool autoInject)
    {
        if (instance is null)
            throw new InvalidOperationException();
        
        var type = instance.GetType();

#if DEBUG
        var hasDependencies = false;
#endif

        foreach (var field in type.GetAllFields())
        {
            if (!Attribute.IsDefined(field, typeof(DependencyAttribute)))
                continue;

#if DEBUG
            hasDependencies = true;
            
            if (field.GetValue(instance) is not null)
            {
                _debugLogger.Warning($"Attempting injecting in {type.Name} already exists {field.FieldType} to {field.Name}");
                continue;
            }   
#endif
            
            field.SetValue(instance, Resolve(field.FieldType));
        }

#if DEBUG
        if (!hasDependencies && !autoInject)
        {
            _debugLogger.Warning($"Attempting injecting in {type.Name} without valid dependencies");
        }
#endif
        
        if (instance is IPostInject postInject)
            postInject.PostInject();
    }

    public object Resolve(Type type)
    {
        lock (_lock)
        {
            if (!_resolutions.Add(type))
                throw new Exception();

            try
            {
                if (_instances.TryGetValue(type, out var instance))
                    return instance;

                if (_factories.ContainsKey(type))
                    return Instantiate(type);
                
                if (_parent is not null)
                    return _parent.Resolve(type);

                throw new KeyNotFoundException();
            }
            finally
            {
                _resolutions.Remove(type);   
            }
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _instances.Clear();
            _factories.Clear();
            _resolutions.Clear();
        }
    }
}
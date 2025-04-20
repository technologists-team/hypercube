using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Hypercube.Core.Ecs.Core.Components;
using Hypercube.Core.Ecs.Core.Events;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Core.Ecs.Core.Utilities;
using Hypercube.Core.Ecs.Events;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Ecs.Core;

/// <inheritdoc/>
[EngineInternal]
public sealed class World : IWorld
{
    /// <inheritdoc/>
    public int Id { get; }
    
    private readonly Dictionary<Type, object> _componentPools = [];
    private readonly Dictionary<Type, IEntitySystem> _systems = [];

    private readonly DependenciesContainer _container;
    private readonly WorldEventBus _eventBus = new();
    private readonly IntPool _entityPool = new();

    public EntityQueryBuilder EntityQueryBuilder => new(this);
    
    public World(int id, List<Type> systems, DependenciesContainer? container = null)
    {
        Id = id;
        _container = new DependenciesContainer(container);

        AddSystems(systems);
    }
    
    /// <inheritdoc/>
    public void Update(float deltaTime)
    {
        foreach (var (_, system) in _systems)
            system.Update(deltaTime);
    }

    #region System

    /// <inheritdoc/>
    public T GetSystem<T>() where T : IEntitySystem
    {
        return (T) _systems[typeof(T)];
    }
    
    #endregion

    #region Entity
    
    /// <inheritdoc/>
    public Entity CreateEntity()
    {
        return new Entity(_entityPool.Next, this);
    }

    /// <inheritdoc/>
    public bool DestroyEntity(Entity entity)
    {
        _entityPool.Release(entity.Id);
        return true; 
    }
    
    #endregion

    #region Component
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AddComponent<T>(Entity entity) where T : IComponent
    {
        var component = InstantiateComponent<T>();
        var result =  GetComponentMapper<T>().Set(entity.Id, component);
        var ev = new AddedEvent();
        
        _eventBus.Raise(entity, component, ref ev);
        return result;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool RemoveComponent<T>(Entity entity) where T : IComponent
    {
        var pool = GetComponentMapper<T>();
        if (!pool.Remove(entity.Id))
            return false;

        var component = pool.Get(entity.Id);
        var ev = new RemovedEvent();
        
        _eventBus.Raise(entity, component, ref ev);
        return true;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity) where T : IComponent
    {
        return GetComponentMapper<T>().Has(entity.Id);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity) where T : IComponent
    {
        return GetComponentMapper<T>().Get(entity.Id);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T EnsureComponent<T>(Entity entity) where T : IComponent
    {
        var pool = GetComponentMapper<T>();
        if (pool.Has(entity.Id))
            return pool.Get(entity.Id);
        
        var component = InstantiateComponent<T>();
        var ev = new AddedEvent();
            
        pool.Set(entity.Id, component);
            
        _eventBus.Raise(entity, component, ref ev);
        return component;
    }
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T? component) where T : IComponent
    {
        component = default;
        return GetComponentMapper<T>().TryGet(entity.Id, ref component);
    }
    
    #endregion

    #region Subscription

    /// <inheritdoc/>
    public void Raise<TComp, TEvent>(Entity entity, TComp component, ref TEvent ev)
        where TComp : IComponent where TEvent : IEvent
    {
        _eventBus.Raise(entity, component, ref ev);
    }

    /// <inheritdoc/>
    public void Subscribe<TComp, TEvent>(EventRefHandler<TComp, TEvent> handler)
        where TComp : IComponent where TEvent : IEvent
    {
        _eventBus.Subscribe(handler);
    }

    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ComponentMapper<T> GetComponentMapper<T>()
        where T : IComponent
    {
        if (_componentPools.TryGetValue(typeof(T), out var pool))
            return (ComponentMapper<T>) pool;
        
        pool = new ComponentMapper<T>();
        _componentPools[typeof(T)] = pool;
        
        return (ComponentMapper<T>) pool;
    }
    
    private void AddSystems(List<Type> types)
    {
        foreach (var type in types)
        {
            var system = InstantiateSystem(type);

            if (!_systems.TryAdd(type, system))
                throw new InvalidOperationException();

            _container.Register(type);
        }

        foreach (var (_, system) in _systems)
        {
            _container.Inject(system);
        }
        
        foreach (var (_, system) in _systems)
        {
            system.Startup();
        }
    }

    private IEntitySystem InstantiateSystem(Type type)
    {
        var constructors = type.GetConstructors();
        if (constructors.Length == 0)
            throw new Exception();

        // It's not assumed that systems have a meaningful constructor
        var constructor = constructors[0];
        var instance = constructor.Invoke(null);
        
        // Since we are working with an interface we cannot use a constructor
        // I don't want to create an initialization method and allow nullable types either
        // So we just set the value to getter
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                   BindingFlags.FlattenHierarchy;
        const string property = nameof(IEntitySystem.World);
        
        // If the property setter is private, it does not exist in the inherited class.
        // Working to go one level below, DeclaringType of the PropertyInfo
        var propertyInfo = type
            .GetProperty(property, flags)?
            .DeclaringType?
            .GetProperty(property, flags);
       
        propertyInfo?.SetValue(instance, this);

        return (IEntitySystem) instance;
    }
    
    private T InstantiateSystem<T>() where T : IEntitySystem
    {
        return (T) InstantiateSystem(typeof(T));
    }

    private T InstantiateComponent<T>() where T : IComponent
    {
        var constructors = typeof(T).GetConstructors();
        if (constructors.Length == 0)
            throw new Exception();
        
        var constructor = constructors[0];
        var instance = constructor.Invoke(null);

        return (T) instance;
    }
}
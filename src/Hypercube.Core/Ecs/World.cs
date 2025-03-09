using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Hypercube.Core.Ecs.Components;
using Hypercube.Core.Ecs.Systems;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.Ecs;

public class World : IWorld
{
    private readonly Dictionary<Type, object> _componentPools = [];
    private readonly Dictionary<Type, IEntitySystem> _systems = [];

    private int _nextEntityId;
    
    public void Update(float deltaTime)
    {
        foreach (var (_, system) in _systems)
            system.Update(deltaTime);
    }

    public bool AddSystem(Type type)
    {
        if (_systems.ContainsKey(type))
            return false;

        _systems.Add(type, InstantiateSystem(type));
        return true;
    }

    public bool AddSystem<T>() where T : IEntitySystem
    {
        if (_systems.ContainsKey(typeof(T)))
            return false;

        _systems.Add(typeof(T), InstantiateSystem<T>());
        return true;
    }

    public T GetSystem<T>() where T : IEntitySystem
    {
        return (T) _systems[typeof(T)];
    }

    public Entity CreateEntity()
    {
        return new Entity(_nextEntityId++);
    }

    public bool DestroyEntity(Entity entity)
    {
        throw new NotImplementedException();
    }

    public bool AddComponent<T>(Entity entity) where T : IComponent
    {
        return GetComponentPool<T>().Set(entity.Id, InstantiateComponent<T>());
    }

    public bool RemoveComponent<T>(Entity entity) where T : IComponent
    {
        return GetComponentPool<T>().Remove(entity.Id);
    }

    public bool HasComponent<T>(Entity entity) where T : IComponent
    {
        return GetComponentPool<T>().Has(entity.Id);
    }

    public T GetComponent<T>(Entity entity) where T : IComponent
    {
        return GetComponentPool<T>().Get(entity.Id);
    }

    public T EnsureComponent<T>(Entity entity) where T : IComponent
    {
        var pool = GetComponentPool<T>();
        if (!pool.Has(entity.Id))
            pool.Set(entity.Id, InstantiateComponent<T>());

        return pool.Get(entity.Id);
    }

    public bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T? component) where T : IComponent
    {
        component = default;
        return GetComponentPool<T>().TryGet(entity.Id, ref component);
    }

    private ComponentPool<T> GetComponentPool<T>() where T : IComponent
    {
        if (_componentPools.TryGetValue(typeof(T), out var pool))
            return (ComponentPool<T>) pool;
        
        pool = new ComponentPool<T>();
        _componentPools[typeof(T)] = pool;
        
        return (ComponentPool<T>) pool;
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
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.FlattenHierarchy;
        
        // If the property setter is private, it does not exist in the inherited class.
        // Working to go one level below, DeclaringType of the PropertyInfo
        var propertyInfo = type
            .GetProperty(nameof(IEntitySystem.World), flags)?
            .DeclaringType?
            .GetProperty(nameof(IEntitySystem.World), flags);
       
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

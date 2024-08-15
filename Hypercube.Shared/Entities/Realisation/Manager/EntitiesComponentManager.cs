using System.Collections.Frozen;
using System.Reflection;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.Entities.Systems.MetaData;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public sealed partial class EntitiesComponentManager : IEntitiesComponentManager, IEventSubscriber, IPostInject 
{
    private static readonly Type BaseComponentType = typeof(IComponent);
    
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IEntitiesEventBus _entitiesEventBus = default!;

    private readonly Dictionary<EntityUid, HashSet<Type>> _entitiesComponentSet = new();
    
    private FrozenDictionary<Type, Dictionary<EntityUid, IComponent>> _entitiesComponents = FrozenDictionary<Type, Dictionary<EntityUid, IComponent>>.Empty;
    private FrozenSet<Type> _components = FrozenSet<Type>.Empty;
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(this, OnInitialized);
        
        _eventBus.Subscribe<EntityAdded>(this, OnEntityAdded);
        _eventBus.Subscribe<EntityRemoved>(this, OnEntityRemoved);
    }

    private void OnEntityAdded(ref EntityAdded args)
    {
        _entitiesComponentSet[args.EntityUid] = new HashSet<Type>();
        
        var added = args;
        AddComponent<MetaDataComponent>(args.EntityUid, entity =>
        {
            entity.Component.Name = added.Name;
        });
        
        AddComponent<TransformComponent>(args.EntityUid, entity =>
        {
            entity.Component.SceneId = added.Coordinates.Scene;
            entity.Component.Transform.SetPosition(added.Coordinates.Position);
        });
    }
    
    private void OnEntityRemoved(ref EntityRemoved args)
    {
        var components = _entitiesComponentSet[args.EntityUid];
        foreach (var component in components)
        {
            RemoveComponent(args.EntityUid, component, false);
        }

        _entitiesComponentSet.Remove(args.EntityUid);
    }

    private void OnInitialized(ref RuntimeInitializationEvent args)
    {
        _components = ReflectionHelper.GetAllInstantiableSubclassOf(BaseComponentType);

        var entitiesComponents = new Dictionary<Type, Dictionary<EntityUid, IComponent>>();
        foreach (var component in _components)
        {
            entitiesComponents.Add(component, new Dictionary<EntityUid, IComponent>());
        }
        
        _entitiesComponents = entitiesComponents.ToFrozenDictionary();
    }
    
    public T AddComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return (T)AddComponent(entityUid, typeof(T));
    }

    public T AddComponent<T>(EntityUid entityUid, Action<Entity<T>> callback) where T : IComponent
    {
        return (T)AddComponent(entityUid, typeof(T), (uid, component) =>
        {
            callback(new Entity<T>(uid, (T)component));
        });
    }

    public void RemoveComponent<T>(EntityUid entityUid) where T : IComponent
    {
        RemoveComponent(entityUid, typeof(T));
    }
    
    public T GetComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return (T)GetComponent(entityUid, typeof(T));
    }

    public bool HasComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return HasComponent(entityUid, typeof(T));
    }
    
    private object AddComponent(EntityUid entityUid, Type type, Action<EntityUid, IComponent>? callback = null)
    {
        if (!Validate(type))
            throw new InvalidOperationException();

        if (!_entitiesComponents.TryGetValue(type, out var components))
            throw new InvalidOperationException();
        
        var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (constructors.Length != 1)
            throw new InvalidOperationException();

        var constructor = constructors[0];
            
        var constructorParams = constructor.GetParameters();
        if (constructorParams.Length != 0)
            throw new InvalidOperationException();

        var instance = (IComponent)constructor.Invoke(Array.Empty<object>()) ?? throw new NullReferenceException();
        instance.Owner = entityUid;
        
        components.Add(entityUid, instance);
        _entitiesComponentSet[entityUid].Add(instance.GetType());
        
        callback?.Invoke(entityUid, instance);
        _eventBus.Raise(new ComponentAdded(entityUid, instance));
        _entitiesEventBus.Raise(entityUid, instance, new ComponentAdded(entityUid, instance));
        
        return instance;
    }

    private void RemoveComponent(EntityUid entityUid, Type type, bool removeFromSet = true)
    {
        if (!Validate(type))
            throw new InvalidOperationException();

        if (!_entitiesComponents.TryGetValue(type, out var components))
            throw new InvalidOperationException();

        if (!_entitiesComponentSet.TryGetValue(entityUid, out var componentSet))
            throw new InvalidOperationException();

        var instance = components[entityUid];
        
        components.Remove(entityUid);
        
        if (removeFromSet)
            componentSet.Remove(type);
        
        _eventBus.Raise(new ComponentRemoved(entityUid, instance));
        _entitiesEventBus.Raise(entityUid, instance, new ComponentRemoved(entityUid, instance));
    }

    private IComponent GetComponent(EntityUid entityUid, Type type)
    {
        if (!Validate(type))
            throw new InvalidOperationException();

        if (!_entitiesComponents.TryGetValue(type, out var components))
            throw new ArgumentOutOfRangeException();

        if (!components.TryGetValue(entityUid, out var component))
            throw new ArgumentOutOfRangeException();

        return component;
    }

    private bool HasComponent(EntityUid entityUid, Type type)
    {
        if (!Validate(type))
            throw new InvalidOperationException();

        return _entitiesComponents.TryGetValue(type, out var components) && components.ContainsKey(entityUid);
    }
    
    private bool Validate(Type type)
    {
        return _components.Contains(type) && type.IsAssignableTo(BaseComponentType);
    }
}
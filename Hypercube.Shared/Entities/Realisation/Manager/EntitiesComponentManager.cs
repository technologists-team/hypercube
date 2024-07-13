using System.Collections.Frozen;
using System.Reflection;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Utilities.Helpers;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public sealed class EntitiesComponentManager : IEntitiesComponentManager, IPostInject, IEventSubscriber
{
    private static readonly Type BaseComponentType = typeof(IComponent);
    
    [Dependency] private readonly IEventBus _eventBus = default!;

    private FrozenDictionary<Type, Dictionary<EntityUid, IComponent>> _entitiesComponents = FrozenDictionary<Type, Dictionary<EntityUid, IComponent>>.Empty;
    private FrozenSet<Type> _components = FrozenSet<Type>.Empty;
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(this, OnInitialized);
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

    public T GetComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return (T)GetComponent(entityUid, typeof(T));
    }

    public bool HasComponent<T>(EntityUid entityUid) where T : IComponent
    {
        return HasComponent(entityUid, typeof(T));
    }
    
    private object AddComponent(EntityUid entityUid, Type type)
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

        components.Add(entityUid, instance);
        _eventBus.Raise(new ComponentAdded(entityUid, instance));

        return instance;
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

    public IEnumerable<Entity<T>> GetEntities<T>() where T : IComponent
    {
        foreach (var (entityUid, component) in _entitiesComponents[typeof(T)])
        {
            yield return new Entity<T>(entityUid, (T)component);
        }
    }
}
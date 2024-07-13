using System.Collections.Frozen;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Utilities.Helpers;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public class EntitiesSystemManager : IEntitiesSystemManager, IPostInject, IEventSubscriber
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly Type _baseSystemType = typeof(IEntitySystem);
    private readonly DependenciesContainer _systemContainer = DependencyManager.Create();

    private FrozenSet<IEntitySystem> _system = FrozenSet<IEntitySystem>.Empty; 
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(this, OnInitialization);
        _eventBus.Subscribe<RuntimeStartupEvent>(this, OnStartup);
        
        _eventBus.Subscribe<UpdateFrameEvent>(this, OnFrameUpdate);
        _eventBus.Subscribe<RuntimeShutdownEvent>(this, OnShutdown);
    }

    private void OnInitialization(ref RuntimeInitializationEvent args)
    {
        // Auto creating all systems
        var types = GetAllSystemTypes();
        foreach (var type in types)
        {
            _systemContainer.Register(type);
        }

        // Creating singleton
        var system = new HashSet<IEntitySystem>();
        foreach (var type in types)
        {
            system.Add((IEntitySystem)_systemContainer.Instantiate(type));
        }

        _system = system.ToFrozenSet();
    }
    
    private void OnStartup(ref RuntimeStartupEvent args)
    {
        foreach (var instance in _system)
        {
            instance.Initialize();
        }   
    }

    private void OnFrameUpdate(ref UpdateFrameEvent args)
    {
        foreach (var instance in _system)
        {
            instance.FrameUpdate(args);
        }
    }
    
    private void OnShutdown(ref RuntimeShutdownEvent args)
    {
        foreach (var instance in _system)
        {
            instance.Shutdown(args);
        }
    }
    
    private FrozenSet<Type> GetAllSystemTypes()
    {
        return ReflectionHelper.GetAllInstantiableSubclassOf(_baseSystemType);
    }
}
using System.Collections.Frozen;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Utilities.Helpers;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public class EntitiesSystemManager : IEntitiesSystemManager, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly Type _baseSystemType = typeof(IEntitySystem);
    private readonly DependenciesContainer _systemContainer = DependencyManager.Create();

    private FrozenSet<IEntitySystem> _system = FrozenSet<IEntitySystem>.Empty; 
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(OnInitialization);
        _eventBus.Subscribe<RuntimeStartupEvent>(OnStartup);
        
        _eventBus.Subscribe<UpdateFrameEvent>(OnFrameUpdate);
        _eventBus.Subscribe<RuntimeShutdownEvent>(OnShutdown);
    }

    private void OnInitialization(RuntimeInitializationEvent args)
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
    
    private void OnStartup(RuntimeStartupEvent args)
    {
        foreach (var instance in _system)
        {
            instance.Initialize();
        }   
    }

    private void OnFrameUpdate(UpdateFrameEvent args)
    {
        foreach (var instance in _system)
        {
            instance.FrameUpdate(args);
        }
    }
    
    private void OnShutdown(RuntimeShutdownEvent args)
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
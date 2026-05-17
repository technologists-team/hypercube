using System.Reflection;
using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Ecs;
using Hypercube.Ecs.System;
using Hypercube.Ecs.System.Collections;
using Hypercube.Utilities.Dependencies;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.Ecs;

public sealed partial class EntitySystemManager : IEntitySystemManager, IPostInject
{
    [Dependency] private readonly IRuntimeLoop _runtimeLoop = null!;

    private readonly SystemSequence<FrameEventArgs> _systemSequence = new();
    
    private readonly DependenciesContainer _container;
    private readonly IWorld _globalWorld;
    
    public EntitySystemManager(IDependenciesContainer container)
    {
        _container = new DependenciesContainer(container);
        _globalWorld = new World();
    }

    public void OnPostInject()
    {
        _runtimeLoop.Actions.Add(OnUpdate, EngineUpdatePriority.EntitySystemUpdate);
    }

    public void Initialize()
    {
        foreach (var type in ReflectionHelper.GetInstantiableSubclasses<EntitySystem>())
        {
            var constructor = type.GetConstructor([]);   
            if (constructor is null) 
                throw new Exception($"Could not find constructor for {type}");

            var instance = constructor.Invoke(null);
            
            // Since we are working with an interface we cannot use a constructor
            // I don't want to create an initialization method and allow nullable types either
            // So we just set the value to getter
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            const string property = nameof(EntitySystem.World);
        
            // If the property setter is private, it does not exist in the inherited class.
            // Working to go one level below, DeclaringType of the PropertyInfo
            var propertyInfo = type
                .GetProperty(property, flags)?
                .DeclaringType?
                .GetProperty(property, flags);
            
            propertyInfo?.SetValue(instance, _globalWorld);
            
            _systemSequence.Add((ISystem<FrameEventArgs>) instance);
            _container.RegisterSingleton(instance.GetType(), instance);
        }
        
        foreach (var system in _systemSequence)
        {
            _container.Inject(system);
        }

        foreach (var system in _systemSequence)
        {
            system.Initialize();
        }
    }

    private void OnUpdate(FrameEventArgs args)
    {
        _systemSequence.BeforeUpdate(args);
        _systemSequence.Update(args);
        _systemSequence.AfterUpdate(args);
    }
}
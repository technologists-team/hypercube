using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Resources.Container;
using Hypercube.Resources.Manager;
using Hypercube.Resources.Preloader;
using Hypercube.Shared.Entities.Realisation.EventBus;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Timing;

namespace Hypercube.Shared;

/// <summary>
/// Provide all shared hypercube dependencies for registration.
/// </summary>
public static class SharedDependencies
{
    public static void Register(DependenciesContainer rootContainer)
    {
        // Timing
        rootContainer.Register<ITiming, Timing.Timing>();
        
        // EventBus
        rootContainer.Register<IEventBus, EventBus.EventBus>();
        
        // Resources
        rootContainer.Register<IResourceLoader, ResourceLoader>();
        rootContainer.Register<IResourceContainer, ResourceContainer>();
        rootContainer.Register<IResourcePreloader, ResourcePreloader>();
        
        // Physics
        rootContainer.Register<IPhysicsManager, PhysicsManager>();
        
        // ECS
        rootContainer.Register<IEntitiesComponentManager, EntitiesComponentManager>();
        rootContainer.Register<IEntitiesSystemManager, EntitiesSystemManager>();
        rootContainer.Register<IEntitiesEventBus, EntitiesEventBus>();
        rootContainer.Register<IEntitiesManager, EntitiesManager>();
        
        rootContainer.InstantiateAll();
    }
}
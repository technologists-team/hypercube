using Hypercube.Client.Audio;
using Hypercube.Client.Audio.Loading;
using Hypercube.Client.Audio.Realisations.OpenAL;
using Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;
using Hypercube.Client.Graphics.Realisation.OpenGL.Texturing;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Client.Input.Handler;
using Hypercube.Client.Input.Manager;
using Hypercube.Client.Resources.Caching;
using Hypercube.Client.Runtimes;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.EventBus;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Resources.Caching;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Timing;

namespace Hypercube.Client;

/// <summary>
/// Provide all client hypercube dependencies for registration.
/// </summary>
public static class Dependencies
{
    public static void Register(DependenciesContainer rootContainer)
    {
        // Timing
        rootContainer.Register<ITiming, Timing>();
        
        // EventBus
        rootContainer.Register<IEventBus, EventBus>();
        
        // Input
        rootContainer.Register<IInputHandler, InputHandler>();
        rootContainer.Register<IInputManager, InputManager>();
        
        // Resources
        rootContainer.Register<IResourceManager, ResourceManager>();
        
        // Audio
        rootContainer.Register<IAudioLoader, AudioLoader>();
        rootContainer.Register<IAudioManager, OpenAlAudioManager>();
        
        // Texturing
        rootContainer.Register<ITextureManager, TextureManager>();
        
        // Caching
        rootContainer.Register<IResourceCacher, ResourceCacher>();
        
        // Camera
        rootContainer.Register<ICameraManager, CameraManager>();
        
        // Physics
        rootContainer.Register<IPhysicsManager, PhysicsManager>();
        
        // Rendering
        rootContainer.Register<IRenderer, Renderer>();
        
        // ECS
        rootContainer.Register<IEntitiesComponentManager, EntitiesComponentManager>();
        rootContainer.Register<IEntitiesSystemManager, EntitiesSystemManager>();
        rootContainer.Register<IEntitiesEventBus, EntitiesEventBus>();
        rootContainer.Register<IEntitiesManager, EntitiesManager>();
        
        // Runtime
        rootContainer.Register<IRuntimeLoop, RuntimeLoop>();
        rootContainer.Register<IRuntime, Runtime>();
        
        rootContainer.InstantiateAll();
    }
}
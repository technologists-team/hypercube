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
using Hypercube.Client.Runtimes;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.EventBus;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Resources.Container;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Resources.Preloader;
using Hypercube.Shared.Runtimes;
using Hypercube.Shared.Runtimes.Loop;
using Hypercube.Shared.Timing;

namespace Hypercube.Client;

/// <summary>
/// Provide all client hypercube dependencies for registration.
/// </summary>
public static class Dependencies
{
    public static void Register(DependenciesContainer rootContainer)
    {

        // Input
        rootContainer.Register<IInputHandler, InputHandler>();
        rootContainer.Register<IInputManager, InputManager>();
        
        // Audio
        rootContainer.Register<IAudioLoader, AudioLoader>();
        rootContainer.Register<IAudioManager, OpenAlAudioManager>();
        
        // Texturing
        rootContainer.Register<ITextureManager, TextureManager>();

        // Camera
        rootContainer.Register<ICameraManager, CameraManager>();
        
        
        // Rendering
        rootContainer.Register<IRenderer, Renderer>();
        
        // Runtime
        rootContainer.Register<IRuntimeLoop, ClientRuntimeLoop>();
        rootContainer.Register<IRuntime, ClientRuntime>();
        
        rootContainer.InstantiateAll();
    }
}
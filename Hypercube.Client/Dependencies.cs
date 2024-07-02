using Hypercube.Client.Graphics;
using Hypercube.Client.Input.Handler;
using Hypercube.Client.Input.Manager;
using Hypercube.Client.Runtimes;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Timing;

namespace Hypercube.Client;

/// <summary>
/// Provide all client hypercube dependencies,
/// for registration.
/// </summary>
public static class Dependencies
{
    public static void Register(DependenciesContainer rootContainer)
    {
        rootContainer.Register<ITiming, Timing>();
        
        rootContainer.Register<IInputHandler, InputHandler>();
        rootContainer.Register<IInputManager, InputManager>();
        
        rootContainer.Register<IRenderer, Renderer>();
        rootContainer.Register<Runtime>(_ => new Runtime(rootContainer));
        rootContainer.Register<RuntimeLoop>();
    }
}
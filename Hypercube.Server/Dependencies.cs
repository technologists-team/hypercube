using Hypercube.Dependencies;
using Hypercube.Runtime;
using Hypercube.Server.Runtimes.Loop;
using Hypercube.Shared;

namespace Hypercube.Server;

/// <summary>
/// Provide all server hypercube dependencies for registration.
/// </summary>
public static class Dependencies
{
    public static void Register(DependenciesContainer rootContainer)
    {
        SharedDependencies.Register(rootContainer);
        
        // Runtime
        rootContainer.Register<IRuntimeLoop, RuntimeLoop>();
        rootContainer.Register<IRuntime, Runtimes.Runtime>();
        
        rootContainer.InstantiateAll();
    }
}
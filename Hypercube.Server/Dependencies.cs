using Hypercube.Server.Runtimes;
using Hypercube.Server.Runtimes.Loop;
using Hypercube.Shared;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Runtimes;
using Hypercube.Shared.Runtimes.Loop;

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
        rootContainer.Register<IRuntimeLoop, ServerRuntimeLoop>();
        rootContainer.Register<IRuntime, ServerRuntime>();
        
        rootContainer.InstantiateAll();
    }
}
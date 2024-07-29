using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Runtimes;
using Hypercube.Shared.Utilities.ArgumentsParser;

namespace Hypercube.Shared;

public abstract class SharedEntryPoint
{
    /// <summary>
    /// The entry point to the engine, starts it in the current thread,
    /// once started, the thread will be frozen by the execution of the loop,
    /// until the engine is shut down. 
    /// </summary>
    /// <param name="args">
    /// Input arguments to the engine.
    /// </param>
    /// <param name="callback">
    /// Callback that will be called after the current <see cref="DependencyManager"/> thread is initialized,
    /// allowing its dependencies to be used, but before entering the game loop.
    /// This is the only way to get into the engine,
    /// not through the  <see cref="IEntitySystem"/>. 
    /// </param>
    public void Enter(string[] args, Action<string[], DependenciesContainer>? callback = null)
    {
        var parser = new ArgumentParser(args);
        parser.TryParse();
        var rootContainer = DependencyManager.InitThread();
        SharedDependencies.Register(rootContainer);
        Enter(parser, rootContainer);
        callback?.Invoke(args, rootContainer);
        var runtime = rootContainer.Resolve<IRuntime>();
        runtime.Run();
    }

    protected abstract void Enter(ArgumentParser parser, DependenciesContainer rootContainer);
}
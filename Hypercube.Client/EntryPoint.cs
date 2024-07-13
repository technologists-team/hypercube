using Hypercube.Client.Runtimes;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Utilities.ArgumentsParser;

namespace Hypercube.Client;

public static class EntryPoint
{
    public static void Enter(string[] args, Action<DependenciesContainer> callback)
    {
        var parser = new ArgumentParser(args);
        parser.TryParse();
        
        var rootContainer = DependencyManager.InitThread();
        Dependencies.Register(rootContainer);
        
        callback.Invoke(rootContainer);
        
        var runtime = rootContainer.Resolve<Runtime>();
        runtime.Run();
    }
}
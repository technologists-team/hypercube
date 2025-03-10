using Hypercube.Core.Ecs;
using Hypercube.Graphics.Rendering;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Execution;

public sealed class RuntimeLoop : IRuntimeLoop
{
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly ILogger _logger = default!;
    [Dependency] private readonly IRenderer _renderer = default!;

    public bool Running { get; private set; }

    public void Run()
    {
        if (Running)
            throw new InvalidOperationException();

        _logger.Debug("Started run loop, suspend main thread");
        
        Running = true;
        while (Running)
            OnRun();
        
        _logger.Debug("Shutdown run loop, unsuspend main thread");
    }

    public void Shutdown()
    {
        Running = false;
    }

    private void OnRun()
    {
        _entitySystemManager.Update(1);
        _renderer.Update();
        _renderer.Render();
    }
}
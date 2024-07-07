using Hypercube.Client.Graphics;
using Hypercube.Client.Graphics.Event;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Event;

namespace Hypercube.Client.Runtimes;

public sealed partial class Runtime(DependenciesContainer dependenciesContainer) : IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IRuntimeLoop _loop = default!;

    private readonly ILogger _logger =  LoggingManager.GetLogger("runtime");

    public void PostInject()
    {
        _eventBus.Subscribe<MainWindowClosedEvent>(OnMainWindowClosed);
    }

    /// <summary>
    /// Start Hypercube.
    /// </summary>
    public void Run()
    {
        Initialize();
        _logger.EngineInfo("Started");
        
        RunLoop();
        
        _logger.EngineInfo("Bye-bye, see you later");
    }
    
    private void Shutdown(string? reason = null)
    {
        // Already got shut down I assume,
        if (!_loop.Running)
            return;

        reason = reason is null ? "Shutting down" : $"Shutting down, reason: {reason}";
        
        _logger.EngineInfo(reason);
        _eventBus.Invoke(new RuntimeShutdownEvent(reason));
        _loop.Shutdown();
    }
    
    private void RunLoop()
    {
        _logger.EngineInfo("Startup");
        _eventBus.Invoke(new RuntimeStartupEvent());
        _loop.Run();
    }
    
    private void Initialize()
    {
        _logger.EngineInfo("Initialize");
        _eventBus.Invoke(new RuntimeInitializationEvent());
    }
    
    private void OnMainWindowClosed(MainWindowClosedEvent obj)
    {
        Shutdown("Main window closed");
    }
}
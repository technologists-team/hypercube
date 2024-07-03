using Hypercube.Client.Graphics;
using Hypercube.Client.Graphics.Event;
using Hypercube.Client.Runtimes.Event;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;

namespace Hypercube.Client.Runtimes;

public sealed partial class Runtime(DependenciesContainer dependenciesContainer) : IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IRenderer _renderer = default!;
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

        _logger.EngineInfo(reason is null ? "Shutting down" : $"Shutting down, reason: {reason}");
        _loop.Shutdown();
    }
    
    private void RunLoop()
    {
        _logger.EngineInfo("Startup");
        _eventBus.Invoke(new StartupEvent());
        _loop.Run();
    }
    
    private void Initialize()
    {
        _logger.EngineInfo("Initialize");
        _eventBus.Invoke(new InitializationEvent());
    }
    
    private void OnMainWindowClosed(MainWindowClosedEvent obj)
    {
        Shutdown("Main window closed");
    }
}
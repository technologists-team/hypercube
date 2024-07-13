﻿using Hypercube.Client.Graphics.Event;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Event;

namespace Hypercube.Client.Runtimes;

public sealed partial class Runtime(DependenciesContainer dependenciesContainer) : IPostInject, IEventSubscriber
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IRuntimeLoop _loop = default!;

    private readonly ILogger _logger =  LoggingManager.GetLogger("runtime");

    public void PostInject()
    {
        _eventBus.Subscribe<MainWindowClosedEvent>(this, OnMainWindowClosed);
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
        _eventBus.Raise(new RuntimeShutdownEvent(reason));
        _loop.Shutdown();
    }
    
    private void RunLoop()
    {
        _logger.EngineInfo("Startup");
        _eventBus.Raise(new RuntimeStartupEvent());
        _eventBus.Raise(new RuntimeAfterStartupEvent());
        _loop.Run();
    }
    
    private void Initialize()
    {
        _logger.EngineInfo("Initialize");
        _eventBus.Raise(new RuntimeInitializationEvent());
    }
    
    private void OnMainWindowClosed(ref MainWindowClosedEvent obj)
    {
        Shutdown("Main window closed");
    }
}
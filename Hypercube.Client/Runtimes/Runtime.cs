using System.Diagnostics.CodeAnalysis;
using Hypercube.Client.Graphics;
using Hypercube.Client.Runtimes.Loop;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;

namespace Hypercube.Client.Runtimes;

public sealed partial class Runtime(DependenciesContainer dependenciesContainer)
{
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly RuntimeLoop _loop = default!;

    private readonly ILogger _logger =  LoggingManager.GetLogger("runtime");
    
    private Thread? _mainThread;

    /// <summary>
    /// Start Hypercube.
    /// </summary>
    public void Run()
    {
        Startup();
        
        _logger.EngineInfo("Started");
        
        _mainThread = new Thread(MainThread)
        {
            IsBackground = false,
            Priority = ThreadPriority.AboveNormal,
            Name = "Hypercube main thread"
        };
        
        if (OperatingSystem.IsWindows())
            _mainThread.SetApartmentState(ApartmentState.STA);
        
        _mainThread.Start();
        _logger.EngineInfo("Main thread started");
        
        // OH YES THE RENDERER GRABS THIS THREAD TO WORK OFF THE WINDOW EVENTS!
        // You are no longer in control here RuntimeLoop, say goodbye to your domain
        _renderer.EnterWindowLoop();
        
        _logger.EngineInfo("Bye-bye, see you later");
    }
    
    public void Shutdown(string? reason = null)
    {
        // Already got shut down I assume,
        if (!_loop.Running)
            return;

        _logger.EngineInfo(reason is null ? "Shutting down" : $"Shutting down, reason: {reason}");
        _loop.Shutdown();
    }

    private void MainThread()
    {
        DependencyManager.InitThread(dependenciesContainer);
        RunLoop();
        
        // Okay, we want to terminate the damn program,
        // we need to exit the loop of the window running in the main thread
        _renderer.TerminateWindowLoop();
    }
    
    private void RunLoop()
    {
        StartupLoop();
        _loop.Run();
    }
    
    private void Startup()
    {
        _logger.EngineInfo("Startup");
        
        _renderer.Startup();
        _renderer.MainWindowClosed += _ =>
        {
            Shutdown("Main window closed");
        };
        
        _loop.Update += _renderer.FrameUpdate;
        _loop.Render += _renderer.RenderFrame;
    }
    
    private void StartupLoop()
    {
        _renderer.StartupLoop();
    }
}
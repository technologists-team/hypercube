using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Input.Handler;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Graphics.Monitors;
using Hypercube.Graphics.Windowing;
using Hypercube.Logging;
using Hypercube.Mathematics.Vectors;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

public sealed unsafe partial class GlfwWindowing : IWindowing
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    [Dependency] private readonly IRenderer _renderer = default!;

    private readonly Logger _logger =  LoggingManager.GetLogger("glfw");
    
    private bool _initialized;
    private bool _running;
    
    private int _nextWindowId = 1;
    
    public bool Init()
    {
        DependencyManager.Inject(this);

        // We don't let GC take our callbacks
        HandleCallbacks();
        
        if (!GlfwInit())
            return false;

        InitMonitors();
        return true;
    }

    public void Shutdown()
    {
        if (!_initialized)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.Terminate();
    }

    public void EnterWindowLoop()
    {
        _logger.EngineInfo("Enter main loop");
        
        // Don't use GLFW.WindowShouldClose because we handle it use GLFW.SetWindowCloseCallback,
        // apply own callback, and close window on it
        _running = true;
        while (_running)
        {
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WaitEvents();
        }
    }

    public void PollEvents()
    {
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.PollEvents();
    }

    public void Terminate()
    {
        _running = false;
        
        // Send an empty event to unfreeze the window
        // so that the lifecycle can be instantly terminated.
        // Otherwise, main app thread will wait for an event that will never come
        // GLFW.PostEmptyEvent();
        
        _logger.EngineInfo("Exit main loop");
    }
    
    public void MakeContextCurrent(WindowHandle? window)
    {
        if (window is not null)
        {
            if (window.IsDisposed)
                return;

            var reg = (GlfwWindowHandle)window;

            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.MakeContextCurrent(reg);
            return;
        }

        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.MakeContextCurrent(null);
    }
    
    public nint GetProcAddress(string procName)
    {
        return OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetProcAddress(procName);
    }

    public void WindowSetMonitor(WindowHandle window, MonitorHandle monitor, Vector2i vector)
    {
        if (window is not GlfwWindowHandle glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowMonitor(
            glfwWindow, 
            (Monitor*) monitor.Pointer, 
            vector.X,
            vector.Y, 
            monitor.Size.X,
            monitor.Size.Y, 
            monitor.RefreshRate);
    }
    
    public void Dispose()
    {
        Shutdown();
    }
}
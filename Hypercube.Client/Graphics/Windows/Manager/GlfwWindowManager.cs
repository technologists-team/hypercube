using Hypercube.Client.Graphics.Windows.Manager.Registrations;
using Hypercube.Client.Input.Handler;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Graphics.Windows.Manager;

public sealed unsafe partial class GlfwWindowManager : IWindowManager
{
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    private readonly ILogger _logger =  LoggingManager.GetLogger("glfw");
    
    private bool _initialized;
    private bool _running;
    private bool _disposed;
    
    private int _nextWindowId = 1;
    
    public bool Init()
    {
        DependencyManager.Inject(this);

        if (!GlfwInit())
            return false;

        InitMonitors();
        return true;
    }

    public void Shutdown()
    {
        if (!_initialized)
            return;
        
        GLFW.Terminate();
    }

    public void EnterWindowLoop()
    {
        _logger.EngineInfo("Enter main loop");
        
        // Don't use GLFW.WindowShouldClose because we handle it use GLFW.SetWindowCloseCallback,
        // apply own callback, and close window on it
        _running = true;
        while (_running)
        {
            GLFW.WaitEvents();
        }
    }

    public void PollEvents()
    {
        GLFW.PollEvents();
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
    
    public void MakeContextCurrent(WindowRegistration? window)
    {
        if (window is not null)
        {
            if (window.IsDisposed)
                return;

            var reg = (GlfwWindowRegistration)window;

            GLFW.MakeContextCurrent(reg.Pointer);
            return;
        }

        GLFW.MakeContextCurrent(null);
    }
    
    public nint GetProcAddress(string procName)
    {
        return GLFW.GetProcAddress(procName);
    }
    
    public void WindowSetTitle()
    {
        throw new NotImplementedException();
    }

    public void WindowSetMonitor()
    {
        throw new NotImplementedException();
    }

    public void WindowRequestAttention()
    {
        throw new NotImplementedException();
    }

    public void WindowSetVisible()
    {
        throw new NotImplementedException();
    }

    public void WindowSwapBuffers(WindowRegistration window)
    {
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        GLFW.SwapBuffers(glfwWindow.Pointer);
    }

    public void Dispose()
    {
        
    }
}
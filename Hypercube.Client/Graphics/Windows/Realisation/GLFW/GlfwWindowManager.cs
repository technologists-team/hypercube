using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Input.Handler;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

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
    
    public void MakeContextCurrent(WindowRegistration? window)
    {
        if (window is not null)
        {
            if (window.IsDisposed)
                return;

            var reg = (GlfwWindowRegistration)window;

            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.MakeContextCurrent(reg.Pointer);
            return;
        }

        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.MakeContextCurrent(null);
    }
    
    public nint GetProcAddress(string procName)
    {
        return OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetProcAddress(procName);
    }
    
    public void WindowSetTitle(WindowRegistration window, string title)
    {
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowTitle(glfwWindow.Pointer, title);
    }

    public void WindowSetMonitor(WindowRegistration window, MonitorRegistration monitor, Vector2Int vector)
    {
        if (monitor is not GlfwMonitorRegistration glfwMonitor)
            return;
        
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowMonitor(
            glfwWindow.Pointer, 
            glfwMonitor.Pointer, 
            vector.X, vector.Y, 
            monitor.Handle.Size.X, monitor.Handle.Size.Y, 
            monitor.Handle.RefreshRate);
    }

    public void WindowSetMonitor(WindowRegistration window, MonitorRegistration registration)
    {
        WindowSetMonitor(window, registration, Vector2Int.Zero);
    }

    public void WindowRequestAttention(WindowRegistration window)
    {
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.RequestWindowAttention(glfwWindow.Pointer);
    }

    public void WindowSetSize(WindowRegistration window, Vector2Int size)
    {
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowSize(glfwWindow.Pointer, size.X, size.Y);
    }

    public void WindowSetVisible(WindowRegistration registration, bool visible)
    {
        if (registration is not GlfwWindowRegistration glfwWindow)
            return;

        if (visible)
        {
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.ShowWindow(glfwWindow.Pointer);
            return;
        }
            
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.HideWindow(glfwWindow.Pointer);
    }

    public void WindowSetOpacity(WindowRegistration window, float opacity)
    {
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowOpacity(glfwWindow.Pointer, opacity);
    }

    public void WindowSwapBuffers(WindowRegistration window)
    {
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SwapBuffers(glfwWindow.Pointer);
    }

    public void Dispose()
    {
        
    }
}
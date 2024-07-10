using Hypercube.Client.Graphics.Event;
using Hypercube.Client.Graphics.OpenGL;
using Hypercube.Client.Graphics.Windows;

namespace Hypercube.Client.Graphics.Rendering;

public sealed partial class Renderer
{
    public WindowRegistration MainWindow => _windows[_mainWindowId];
    public IReadOnlyDictionary<WindowId, WindowRegistration> Windows => _windows; 
    
    private readonly Dictionary<WindowId, WindowRegistration> _windows = new();
    private WindowId _mainWindowId = WindowId.Invalid;
    
    public void EnterWindowLoop()
    {
        _windowManager.EnterWindowLoop();
    }

    public void TerminateWindowLoop()
    {
        _windowManager.Terminate();
    }

    public WindowRegistration CreateWindow(WindowCreateSettings settings)
    {
        var (registration, error) = CreateWindow(_context, settings, MainWindow);
        if (registration is null)
            throw new Exception(error);

        return registration;
    }
    
    public void DestroyWindow(WindowRegistration registration)
    {
        _windowManager.WindowDestroy(registration);
        _windows.Remove(registration.Id);
    }

    public void CloseWindow(WindowRegistration registration)
    {
        _eventBus.Invoke(new WindowClosedEvent(registration));
        
        if (registration.Id == _mainWindowId)
        {
            _eventBus.Invoke(new MainWindowClosedEvent(registration));
            return;
        }
        
        if (!registration.DisposeOnClose)
            return;
        
        DestroyWindow(registration);
    }
    
    private bool InitMainWindow(ContextInfo? context, WindowCreateSettings settings)
    {
        var (registration, error) = CreateWindow(context, settings, null);
        if (registration is null)
        {
            _logger.Error($"Initializing main window failed, error:\n{error}");
            return false;
        }
        
        _mainWindowId = registration.Id;
        return true;
    }

    private (WindowRegistration? registration, string? error) CreateWindow(ContextInfo? context, WindowCreateSettings settings, WindowRegistration? share)
    {
        var result = _windowManager.WindowCreate(context, settings, share);
        if (result.Failed)
            return (null, result.Error);

        var registration = result.Registration;
        
        _windowManager.MakeContextCurrent(registration);
        _windows.Add(registration.Id, registration);
        
        _logger.EngineInfo($"Created new window {registration.Id}");
        
        return (registration, null);
    }

    public void OnFocusChanged(WindowRegistration window, bool focused)
    {
        _eventBus.Invoke(new WindowFocusChangedEvent(window, focused));
    }
}
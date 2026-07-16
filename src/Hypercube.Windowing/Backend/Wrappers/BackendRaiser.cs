using Hypercube.Windowing.Backend.Events;
using Hypercube.Windowing.Backend.Handlers;

namespace Hypercube.Windowing.Backend.Wrappers;

public sealed class BackendRaiser
{
    public event ErrorHandler? OnError;
    public event MonitorStateHandler? OnMonitor;
    public event JoystickStateHandler? OnJoystick;
    public event WindowCloseHandler? OnWindowClose;
    public event WindowSizeHandler? OnWindowSize;
    public event WindowFramebufferSizeHandler? OnWindowFramebufferSize;
    public event WindowPositionHandler? OnWindowPosition;
    public event WindowFocusHandler? OnWindowFocus;
    public event WindowInputKeyHandler? OnWindowInputKey;
    public event WindowInputCursorHandler? OnWindowInputCursor;
    public event WindowInputMouseButtonHandler? OnWindowInputMouseButton;
    public event WindowInputScrollHandler? OnWindowInputScroll;
    
    public void Raise(IEvent raw)
    {
        switch (raw)
        {
            case EventError ev:
                OnError?.Invoke(ev.Message, ev.Code);
                break;
                
            case EventJoystick ev:
                OnJoystick?.Invoke(ev.Joystick, ev.State);
                break;

            case EventMonitor ev:
                OnMonitor?.Invoke(ev.Monitor, ev.State);
                break;

            case EventWindowClose ev:
                OnWindowClose?.Invoke(ev.Window);
                break;
                
            case EventWindowSize ev:
                OnWindowSize?.Invoke(ev.Window, ev.Size);
                break;
            
            case EventWindowPosition ev:
                OnWindowPosition?.Invoke(ev.Window, ev.Position);
                break;

            case EventWindowFocus ev:
                OnWindowFocus?.Invoke(ev.Window, ev.Focused);
                break;
            
            case EventWindowInputKey ev:
                OnWindowInputKey?.Invoke(ev.Window, ev.Key, ev.State, ev.Modifiers, ev.ScanCode);
                break;
                
            case EventWindowInputCursorPosition ev:
                OnWindowInputCursor?.Invoke(ev.Window, ev.Position);
                break;
            
            case EventWindowInputMouseButton ev:
                OnWindowInputMouseButton?.Invoke(ev.Window, ev.Button, ev.State, ev.Modifiers);
                break;

            case EventWindowInputScroll ev:
                OnWindowInputScroll?.Invoke(ev.Window, ev.Offset);
                break;
        }
    }
}

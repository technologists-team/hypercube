using Hypercube.Mathematics.Vectors;
// Silk redefine for reduce type/namespace collisions
using SilkErrorCode = Silk.NET.GLFW.ErrorCode;
using SilkWindow = Silk.NET.GLFW.WindowHandle;
using SilkMonitor = Silk.NET.GLFW.Monitor;
using SilkConnected = Silk.NET.GLFW.ConnectedState;
using SilkKeys = Silk.NET.GLFW.Keys;
using SilkKeyModifiers = Silk.NET.GLFW.KeyModifiers;
using SilkInputAction = Silk.NET.GLFW.InputAction;
using SilkMouseButton = Silk.NET.GLFW.MouseButton;

namespace Hypercube.Windowing.Backend.Realisation.Glfw;

public sealed unsafe partial class GlfwBackend
{
    private void ErrorCallback(SilkErrorCode code, string description)
    {
        OnError?.Invoke(description, Translate(code));
    }
    
    private void MonitorCallback(SilkMonitor* monitor, SilkConnected state)
    {
        OnMonitorState?.Invoke(Translate(monitor), Translate(state));
    }
    
    private void JoystickCallback(int joystick, SilkConnected state)
    {
        OnJoystickState?.Invoke(joystick, Translate(state));
    }

    #region Window input

    private void WindowKeyCallback(SilkWindow* window, SilkKeys key, int scancode, SilkInputAction action, SilkKeyModifiers mods)
    {
        OnWindowInputKey?.Invoke(Translate(window), Translate(key), Translate(action), Translate(mods), scancode);
    }

    private void WindowCursorCallback(SilkWindow* window, double x, double y)
    {
        OnWindowInputCursor?.Invoke(Translate(window), new Vector2d(x, y));
    }

    private void WindowMouseButtonCallback(SilkWindow* window, SilkMouseButton button, SilkInputAction action, SilkKeyModifiers mods)
    {
        OnWindowInputMouseButton?.Invoke(Translate(window), Translate(button), Translate(action), Translate(mods));
    }

    private void WindowScrollCallback(SilkWindow* window, double x, double y)
    {
        OnWindowInputScroll?.Invoke(Translate(window), new Vector2d(x, y));
    }

    #endregion
    
    #region Window state

    private void WindowCloseCallback(SilkWindow* window)
    {
        OnWindowClose?.Invoke(Translate(window));
    }
    
    private void WindowSizeCallback(SilkWindow* window, int width, int height)
    {
        OnWindowSize?.Invoke(Translate(window), new Vector2i(width, height));
    }
    
    private void WindowFramebufferSizeCallback(SilkWindow* window, int width, int height)
    {
        OnWindowFramebufferSize?.Invoke(Translate(window), new Vector2i(width, height));
    }
    
    private void WindowPositionCallback(SilkWindow* window, int x, int y)
    {
        OnWindowPosition?.Invoke(Translate(window), new Vector2i(x, y));
    }
    
    private void WindowFocusCallback(SilkWindow* window, bool focused)
    {
        OnWindowFocus?.Invoke(Translate(window), focused);
    }

    #endregion
}

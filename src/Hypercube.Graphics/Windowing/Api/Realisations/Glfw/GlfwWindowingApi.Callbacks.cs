using Hypercube.Mathematics.Vectors;
using Silk.NET.GLFW;
using Monitor = Silk.NET.GLFW.Monitor;

namespace Hypercube.Graphics.Windowing.Api.Realisations.Glfw;

public unsafe partial class GlfwWindowingApi
{
    private void OnErrorCallback(ErrorCode errorCode, string description)
    {
        Raise(new EventError($"Glfw internal error {errorCode}: {description}"));
    }
    
    private void OnMonitorCallback(Monitor* monitor, ConnectedState state)
    {
        Raise(new EventMonitor((nint) monitor, FromConnectedState(state)));
    }
    
    private void OnJoystickCallback(int joystick, ConnectedState state)
    {
        Raise(new EventJoystick(joystick, FromConnectedState(state)));
    }

    private void OnWindowCloseCallback(WindowHandle* window)
    {   
        Raise(new EventWindowClose((nint) window));
    }

    private void OnWindowSizeCallback(WindowHandle* window, int width, int height)
    {
        Raise(new EventWindowSize((nint) window, new Vector2i(width, height)));
    }

    private void OnWindowPositionCallback(WindowHandle* window, int x, int y)
    {
        Raise(new EventWindowPosition((nint) window, new Vector2i(x, y)));
    }

    private void OnWindowFocusCallback(WindowHandle* window, bool focused)
    {
        Raise(new EventWindowFocus((nint) window, focused));
    }
}
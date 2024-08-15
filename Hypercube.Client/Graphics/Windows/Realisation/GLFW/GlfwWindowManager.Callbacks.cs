using Hypercube.Client.Input.Events.Windowing;
using Hypercube.EventBus.Events;
using Hypercube.Input;
using Hypercube.OpenGL.Utilities.Helpers;
using OpenTK.Windowing.GraphicsLibraryFramework;

using GlfwKeyModifiers = OpenTK.Windowing.GraphicsLibraryFramework.KeyModifiers;
using GlfwMouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;

using KeyModifiers = Hypercube.Input.KeyModifiers;
using MouseButton = Hypercube.Input.MouseButton;

using static OpenTK.Windowing.GraphicsLibraryFramework.GLFWCallbacks;

namespace Hypercube.Client.Graphics.Windows.Realisation.Glfw;

public sealed unsafe partial class GlfwWindowManager
{
    private ErrorCallback? _errorCallback;

    private CharCallback? _charCallback;
    private ScrollCallback? _scrollCallback;
    private KeyCallback? _keyCallback;
    private MouseButtonCallback? _mouseButtonCallback;

    private WindowCloseCallback? _windowCloseCallback;
    private WindowSizeCallback? _windowSizeCallback;
    private WindowFocusCallback? _windowFocusCallback;
    
    /// <summary>
    /// GC doesn't think our callbacks are stored anywhere,
    /// so it cleans them up,
    /// we need to indicate that there is a link to them so they don't get lost.
    /// </summary>
    private void HandleCallbacks()
    {
        _errorCallback = OnErrorHandled;

        _charCallback = OnWindowCharHandled;
        _scrollCallback = OnWindowScrollHandled;
        _keyCallback = OnWindowKeyHandled;
        _mouseButtonCallback = OnMouseButtonHandled;

        _windowCloseCallback = OnWindowClosed;
        _windowSizeCallback = OnWindowResized;
        _windowFocusCallback = OnWindowFocusChanged;
    }

    private void OnErrorHandled(ErrorCode error, string description)
    {
        _logger.Error(GLFWHelper.FormatError(error, description));
    }

    #region Input handler
    
    private void OnWindowKeyHandled(Window* window, Keys glfwKey, int scanCode, InputAction action, GlfwKeyModifiers mods)
    {
        RaiseInput(new WindowingKeyHandledEvent(
            (Key)glfwKey,
            Convert(action),
            (KeyModifiers)mods,
            scanCode
        ));
    }
    
    private void OnWindowCharHandled(Window* window, uint codepoint)
    {
        RaiseInput(new WindowingCharHandledEvent());
    }
    
    private void OnWindowScrollHandled(Window* window, double offsetX, double offsetY)
    {
        RaiseInput(new WindowingScrollHandledEvent());
    }
    
    private void OnMouseButtonHandled(Window* window, GlfwMouseButton button, InputAction action, GlfwKeyModifiers mods)
    {
        RaiseInput(new WindowingMouseButtonHandledEvent(
            (MouseButton)button,
            Convert(action),
            (KeyModifiers)mods
        ));
    }

    private void RaiseInput<T>(T args) where T : IEventArgs
    {
        _eventBus.Raise(_inputHandler, ref args);
    }
    
    #endregion
    
    private void OnWindowClosed(Window* window)
    {
        if (!TryGetWindow(window, out var registration))
            return;

        _renderer.CloseWindow(registration);
    }

    private void OnWindowResized(Window* window, int width, int height)
    {
        if (!TryGetWindow(window, out var registration))
            return;

        registration.SetSize(width, height);
    }

    private void OnWindowFocusChanged(Window* window, bool focused)
    {
        if (!TryGetWindow(window, out var registration))
            return;
        
        _renderer.OnFocusChanged(registration, focused);
    }
    
    private void OnWindowFocused(Window* window, bool focused)
    {
        if (!TryGetWindow(window, out var registration))
            return;
        
        _renderer.OnFocusChanged(registration, focused);
    }

    private static KeyState Convert(InputAction action)
    {
        return action switch
        {
            InputAction.Release => KeyState.Release,
            InputAction.Press => KeyState.Pressed,
            InputAction.Repeat => KeyState.Held,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
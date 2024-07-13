using Hypercube.Client.Input;
using Hypercube.Client.Utilities;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwKeyModifiers = OpenTK.Windowing.GraphicsLibraryFramework.KeyModifiers;
using KeyModifiers = Hypercube.Client.Input.KeyModifiers;
using static OpenTK.Windowing.GraphicsLibraryFramework.GLFWCallbacks;

namespace Hypercube.Client.Graphics.Windows.Manager;

public sealed unsafe partial class GlfwWindowManager
{
    private ErrorCallback? _errorCallback;
    
    private KeyCallback? _keyCallback;
    
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
        
        _keyCallback = OnWindowKeyHandled;

        _windowCloseCallback = OnWindowClosed;
        _windowSizeCallback = OnWindowResized;
        _windowFocusCallback = OnWindowFocusChanged;
    }
    
    private void OnErrorHandled(ErrorCode error, string description)
    {
        _logger.Error(GLFWHelper.FormatError(error, description));
    }
    
    private void OnWindowKeyHandled(Window* window, Keys glfwKey, int scanCode, InputAction action, GlfwKeyModifiers mods)
    {
        var key = (Key)glfwKey;
        var pressed = false;
        var repeat = false;
        
        switch (action)
        {
            case InputAction.Release:
                pressed = false;
                break;
            
            case InputAction.Press:
                pressed = true;
                break;
            
            case InputAction.Repeat:
                pressed = true;
                repeat = true;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
        
        _inputHandler.SendKeyState(new KeyStateChangedArgs(
            key,
            pressed,
            repeat,
            (KeyModifiers)mods,
            scanCode));
    }
    
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
    
}
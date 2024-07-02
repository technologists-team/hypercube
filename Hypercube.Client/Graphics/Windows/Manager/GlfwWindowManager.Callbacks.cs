using Hypercube.Client.Input;
using Hypercube.Client.Utilities;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwKeyModifiers = OpenTK.Windowing.GraphicsLibraryFramework.KeyModifiers;
using KeyModifiers = Hypercube.Client.Input.KeyModifiers;

namespace Hypercube.Client.Graphics.Windows.Manager;

public sealed unsafe partial class GlfwWindowManager
{
    private void OnWindowClosed(Window* window)
    {
        if (!TryGetWindow(window, out var registration))
            return;

        _renderer.CloseWindow(registration);
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
        
        _inputHandler.SendKeyState(new KeyStateArgs(
            key,
            pressed,
            repeat,
            (KeyModifiers)mods,
            scanCode));
    }
}
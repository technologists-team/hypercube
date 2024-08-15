using Hypercube.Client.Graphics.Windows;
using Hypercube.EventBus;
using Hypercube.Input;

namespace Hypercube.Client.Input.Handler;

/// <summary>
/// Receives requests from user input, via <see cref="IWindowManager"/>,
/// and submits them for further work via events.
/// </summary>
public interface IInputHandler : IEventSubscriber
{
    bool IsKeyState(Key key, KeyState state);
    bool IsKeyHeld(Key key);
    bool IsKeyPressed(Key key);
    bool IsKeyReleased(Key key);
    void KeyClear();
    
    bool IsMouseButtonState(MouseButton button, KeyState state);
    bool IsMouseButtonHeld(MouseButton button);
    bool IsMouseButtonPressed(MouseButton button);
    bool IsMouseButtonReleased(MouseButton button);
    void MouseButtonClear();
}
using Hypercube.Client.Graphics.Windows;
using Hypercube.Client.Input.Events;
using Hypercube.EventBus;
using Hypercube.Input;
using JetBrains.Annotations;

namespace Hypercube.Client.Input.Handler;

/// <summary>
/// Receives requests from user input, via <see cref="IWindowManager"/>,
/// and submits them for further work via events.
/// </summary>
[PublicAPI]
public interface IInputHandler : IEventSubscriber
{
    bool IsKeyState(Key key, KeyState state);
    
    /// <summary>
    /// Checks the state <see cref="Key"/> is <see cref="KeyState.Held"/> or <see cref="KeyState.Pressed"/>.
    /// </summary>
    bool IsKeyHeld(Key key);
    
    /// <summary>
    /// Checks the state <see cref="Key"/> is <see cref="KeyState.Pressed"/>.
    /// </summary>
    bool IsKeyPressed(Key key);
    
    /// <summary>
    /// Checks the state <see cref="Key"/> is <see cref="KeyState.Released"/>.
    /// </summary>
    bool IsKeyReleased(Key key);
    
    /// <summary>
    /// Clears all received <see cref="KeyState"/> of <see cref="Key"/>,
    /// and can abort all checks and <see cref="KeyHandledEvent"/> (<see cref="KeyState.Held"/>) events.
    /// </summary>
    void KeyClear();
    
    bool IsMouseButtonState(MouseButton button, KeyState state);
    
    /// <summary>
    /// Checks the state <see cref="MouseButton"/> is <see cref="KeyState.Held"/> or <see cref="KeyState.Pressed"/>.
    /// </summary>
    bool IsMouseButtonHeld(MouseButton button);
    
    /// <summary>
    /// Checks the state <see cref="MouseButton"/> is <see cref="KeyState.Pressed"/>.
    /// </summary>
    bool IsMouseButtonPressed(MouseButton button);
    
    /// <summary>
    /// Checks the state <see cref="MouseButton"/> is <see cref="KeyState.Released"/>.
    /// </summary>
    bool IsMouseButtonReleased(MouseButton button);
    
    /// <summary>
    /// Clears all received <see cref="KeyState"/> of <see cref="MouseButton"/>,
    /// and can abort all checks and <see cref="MouseButtonHandledEvent"/> (<see cref="KeyState.Held"/>) events.
    /// </summary>
    void MouseButtonClear();
}
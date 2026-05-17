using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Core.Input.Args;
using WindowHandle = Hypercube.Core.Windowing.Windows.WindowHandle;

namespace Hypercube.Core.Input.Handler;

/// <summary>
/// Interface for handling and simulating keyboard and mouse input states
/// across one or multiple windows.
/// Provides low-level access to input state queries and input simulation.
/// </summary>
[PublicAPI]
public interface IInputHandler : IRuntimeUpdatable
{
    Vector2i MousePosition { get; }
    
    /// <summary>
    /// Clears all recorded key input states (Held and Released keys)
    /// for all tracked windows.
    /// </summary>
    void Clear();

    /// <summary>
    /// Clears all recorded keyboard key states for all tracked windows.
    /// </summary>
    void ClearKeyState();
    
    /// <summary>
    /// Clears all recorded mouse button states for all tracked windows.
    /// </summary>
    void ClearMouseButtonState();
    
    /// <summary>
    /// Checks if the specified key in a given window is currently
    /// in the specified state.
    /// </summary>
    /// <param name="window">The native handle of the window to check.</param>
    /// <param name="key">The key to query.</param>
    /// <param name="state">The key state to compare against.</param>
    /// <returns>
    /// True if the key is in the specified state; otherwise, false.
    /// </returns>
    bool IsKeyState(WindowHandle window, Key key, KeyState state);
    
    /// <summary>
    /// Returns whether the specified key is currently held down
    /// in the given window.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="key">The key to query.</param>
    /// <returns>
    /// True if the key is held; otherwise, false.
    /// </returns>
    bool IsKeyHeld(WindowHandle window, Key key);
    
    /// <summary>
    /// Returns whether the specified key was pressed during
    /// the current frame in the given window.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="key">The key to query.</param>
    /// <returns>
    /// True if the key was pressed this frame; otherwise, false.
    /// </returns>
    bool IsKeyPressed(WindowHandle window, Key key);
    
    /// <summary>
    /// Returns whether the specified key was released during
    /// the current frame in the given window.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="key">The key to query.</param>
    /// <returns>
    /// True if the key was released this frame; otherwise, false.
    /// </returns>
    bool IsKeyReleased(WindowHandle window, Key key);
    
    /// <summary>
    /// Checks if the specified key in the currently active window
    /// is in the given state.
    /// </summary>
    /// <param name="key">The key to query.</param>
    /// <param name="state">The key state to compare against.</param>
    /// <returns>
    /// True if the key is in the specified state; otherwise, false.
    /// </returns>
    bool IsKeyState(Key key, KeyState state);
    
    /// <summary>
    /// Returns whether the specified key is currently held down
    /// in the currently active window.
    /// </summary>
    /// <param name="key">The key to query.</param>
    /// <returns>
    /// True if the key is held; otherwise, false.
    /// </returns>
    bool IsKeyHeld(Key key);
    
    /// <summary>
    /// Returns whether the specified key was pressed during
    /// the current frame in the currently active window.
    /// </summary>
    /// <param name="key">The key to query.</param>
    /// <returns>
    /// True if the key was pressed this frame; otherwise, false.
    /// </returns>
    bool IsKeyPressed(Key key);
    
    /// <summary>
    /// Returns whether the specified key was released during
    /// the current frame in the currently active window.
    /// </summary>
    /// <param name="key">The key to query.</param>
    /// <returns>
    /// True if the key was released this frame; otherwise, false.
    /// </returns>
    bool IsKeyReleased(Key key);
    
    /// <summary>
    /// Simulates a mouse button state change event
    /// in the currently active window.
    /// Typically used for testing or synthetic input.
    /// </summary>
    /// <param name="state">
    /// The mouse button state change to simulate.
    /// </param>
    void SimulateMouseButton(KeyChangedArgs state);
    
    /// <summary>
    /// Simulates a mouse button state change event
    /// in a specific window.
    /// Typically used for testing or synthetic input
    /// in multi-window setups.
    /// </summary>
    /// <param name="window">
    /// The native handle of the window to simulate the event for.
    /// </param>
    /// <param name="state">
    /// The mouse button state change to simulate.
    /// </param>
    void SimulateMouseButton(WindowHandle window, KeyChangedArgs state);

    /// <summary>
    /// Returns whether the specified mouse button is currently held
    /// in the currently active window.
    /// </summary>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>
    /// True if the button is held; otherwise, false.
    /// </returns>
    bool IsMouseButtonHeld(MouseButton button);
    
    /// <summary>
    /// Returns whether the specified mouse button was pressed
    /// during the current frame in the currently active window.
    /// </summary>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>
    /// True if the button was pressed this frame; otherwise, false.
    /// </returns>
    bool IsMouseButtonPressed(MouseButton button);
    
    /// <summary>
    /// Returns whether the specified mouse button was released
    /// during the current frame in the currently active window.
    /// </summary>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>
    /// True if the button was released this frame; otherwise, false.
    /// </returns>
    bool IsMouseButtonReleased(MouseButton button);
    
    /// <summary>
    /// Checks if the specified mouse button in the currently active window
    /// is in the given state.
    /// </summary>
    /// <param name="button">The mouse button to query.</param>
    /// <param name="state">The button state to compare against.</param>
    /// <returns>
    /// True if the button is in the specified state; otherwise, false.
    /// </returns>
    bool IsMouseButtonState(MouseButton button, KeyState state);
    
    /// <summary>
    /// Simulates a mouse button state change event
    /// in the currently active window.
    /// </summary>
    /// <param name="state">
    /// The mouse button state change to simulate.
    /// </param>
    void SimulateMouseButton(MouseButtonChangedArgs state);
    
    /// <summary>
    /// Returns whether the specified mouse button is currently held
    /// in the given window.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>
    /// True if the button is held; otherwise, false.
    /// </returns>
    bool IsMouseButtonHeld(WindowHandle window, MouseButton button);
    
    /// <summary>
    /// Returns whether the specified mouse button was pressed
    /// during the current frame in the given window.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>
    /// True if the button was pressed this frame; otherwise, false.
    /// </returns>
    bool IsMouseButtonPressed(WindowHandle window, MouseButton button);
    
    /// <summary>
    /// Returns whether the specified mouse button was released
    /// during the current frame in the given window.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="button">The mouse button to query.</param>
    /// <returns>
    /// True if the button was released this frame; otherwise, false.
    /// </returns>
    bool IsMouseButtonReleased(WindowHandle window, MouseButton button);
    
    /// <summary>
    /// Checks if the specified mouse button in a given window
    /// is in the specified state.
    /// </summary>
    /// <param name="window">The native window handle.</param>
    /// <param name="button">The mouse button to query.</param>
    /// <param name="state">The button state to compare against.</param>
    /// <returns>
    /// True if the button is in the specified state; otherwise, false.
    /// </returns>
    bool IsMouseButtonState(WindowHandle window, MouseButton button, KeyState state);
    
    /// <summary>
    /// Simulates a mouse button state change event
    /// in a specific window.
    /// </summary>
    /// <param name="window">
    /// The native handle of the window to simulate the event for.
    /// </param>
    /// <param name="state">
    /// The mouse button state change to simulate.
    /// </param>
    void SimulateMouseButton(WindowHandle window, MouseButtonChangedArgs state);

    Vector2i GetMousePosition(WindowHandle window);
}

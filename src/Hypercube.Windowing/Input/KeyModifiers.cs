using JetBrains.Annotations;

namespace Hypercube.Windowing.Input;

/// <summary>
/// Specifies the modifier keys that are currently pressed during a keyboard event.
/// </summary>
[PublicAPI, Flags]
public enum KeyModifiers : short
{
    /// <summary>
    /// No modifier keys are pressed.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// The Shift key is pressed.
    /// </summary>
    Shift = 1 << 0,
    
    /// <summary>
    /// The Control (Ctrl) key is pressed.
    /// </summary>
    Control = 1 << 1,
    
    /// <summary>
    /// The Alt key is pressed.
    /// </summary>
    Alt = 1 << 2,
    
    /// <summary>
    /// The Super key is pressed. 
    /// This corresponds to the Windows key on Windows, the Command key on macOS, and the Meta key on Linux.
    /// </summary>
    Super = 1 << 3,
    
    /// <summary>
    /// The Caps Lock key is toggled on.
    /// </summary>
    CapsLock = 1 << 4,
    
    /// <summary>
    /// The Num Lock key is toggled on.
    /// </summary>
    NumLock = 1 << 5
}

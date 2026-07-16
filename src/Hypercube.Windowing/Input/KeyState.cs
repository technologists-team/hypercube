using JetBrains.Annotations;

namespace Hypercube.Windowing.Input;

/// <summary>
/// Represents the physical state of a keyboard key during an input event.
/// </summary>
[PublicAPI]
public enum KeyState : byte
{
    /// <summary>
    /// The key was just pressed down (transition from released to pressed).
    /// </summary>
    Pressed,    
    
    /// <summary>
    /// The key is being held down. This state is typically used to indicate key repeat events.
    /// </summary>
    Held,
    
    /// <summary>
    /// The key was released (transition from pressed to released).
    /// </summary>
    Released
}

using JetBrains.Annotations;

namespace Hypercube.Windowing.Input;

/// <summary>
/// Identifies a specific button on a mouse or pointing device.
/// </summary>
[PublicAPI]
public enum MouseButton
{
    /// <summary>
    /// The primary (left) mouse button.
    /// </summary>
    Left = 0,
    
    /// <summary>
    /// The secondary (right) mouse button.
    /// </summary>
    Right = 1,
    
    /// <summary>
    /// The middle mouse button (typically the scroll wheel click).
    /// </summary>
    Middle = 2,
    
    /// <summary>
    /// The fourth mouse button (often the "Back" button on the side).
    /// </summary>
    Button4 = 3,
    
    /// <summary>
    /// The fifth mouse button (often the "Forward" button on the side).
    /// </summary>
    Button5 = 4,
    
    /// <summary>
    /// The sixth extended mouse button.
    /// </summary>
    Button6 = 5,
    
    /// <summary>
    /// The seventh extended mouse button.
    /// </summary>
    Button7 = 6,
    
    /// <summary>
    /// The eighth extended mouse button.
    /// </summary>
    Button8 = 7
}

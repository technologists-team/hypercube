namespace Hypercube.Windowing.Core;

/// <summary>
/// Specifies the connection state of a physical device, such as a monitor or joystick.
/// </summary>
public enum ConnectedState : byte
{
    /// <summary>
    /// The device is connected and available.
    /// </summary>
    Connected,
    
    /// <summary>
    /// The device is disconnected and no longer available.
    /// </summary>
    Disconnected
}

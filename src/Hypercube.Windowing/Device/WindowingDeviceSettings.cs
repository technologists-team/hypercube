namespace Hypercube.Windowing.Device;

/// <summary>
/// Contains configuration settings for initializing a windowing device.
/// </summary>
public struct WindowingDeviceSettings
{
    /// <summary>
    /// Gets the underlying windowing backend type to initialize.
    /// </summary>
    public WindowingBackendType Backend { get; init; }

    public bool BackendForced { get; init; }
    
    /// <summary>
    /// Gets a value indicating whether the windowing device should process events on a dedicated background thread.
    /// </summary>
    public bool Multithread { get; init; }
}

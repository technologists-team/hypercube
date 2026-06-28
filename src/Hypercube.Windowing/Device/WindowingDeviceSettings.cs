using Hypercube.Windowing.Types;

namespace Hypercube.Windowing.Device;

public struct WindowingDeviceSettings
{
    public WindowingBackendType Backend { get; init; }
    public bool Multithread { get; init; }
}

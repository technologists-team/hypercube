using Hypercube.Windowing.Core.Device;
using Hypercube.Windowing.Device;

namespace Hypercube.Windowing;

public static class WindowingContext
{
    public static IWindowingDevice Create(in WindowingDeviceSettings deviceSettings) => new WindowingDevice(deviceSettings);
}

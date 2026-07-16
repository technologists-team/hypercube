namespace Hypercube.Graphics.Core.Device;

public abstract class GraphicDeviceModule
{
    protected readonly GraphicDevice Device;

    protected GraphicDeviceModule(GraphicDevice device)
    {
        Device = device;
    }
}

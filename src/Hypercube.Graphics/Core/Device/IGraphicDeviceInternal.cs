using Hypercube.Graphics.Core.Device.Modules;
using Hypercube.Graphics.Device;

namespace Hypercube.Graphics.Core.Device;

public interface IGraphicDeviceInternal : IGraphicDevice
{
    GraphicDeviceBackend Backend { get; }
    GraphicDeviceStatistic Statistic { get; }
    GraphicDeviceResources Resources { get; }
    GraphicDeviceBatcher Batcher { get; }
    GraphicDeviceRenderer Renderer { get; }
}

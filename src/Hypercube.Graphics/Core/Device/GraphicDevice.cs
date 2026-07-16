using Hypercube.Graphics.Core.Device.Modules;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Types;

namespace Hypercube.Graphics.Core.Device;

public sealed class GraphicDevice : IGraphicDeviceInternal
{
    public readonly BackendType BackendType;
    
    public GraphicDeviceBackend Backend { get; }
    public GraphicDeviceStatistic Statistic { get; }
    public GraphicDeviceResources Resources { get; }
    public GraphicDeviceBatcher Batcher { get; }
    public GraphicDeviceRenderer Renderer { get; }
    
    public GraphicDevice(in GraphicsDeviceSettings settings)
    {
        if (settings.Backend is null or BackendType.None)
            throw new ArgumentNullException(nameof(settings.Backend));

        BackendType = settings.Backend.Value;
        
        // Modules
        Backend = new GraphicDeviceBackend(this, settings);
        Statistic = new GraphicDeviceStatistic(this);
        Resources = new GraphicDeviceResources(this);
        Renderer = new GraphicDeviceRenderer(this);
        Batcher = new GraphicDeviceBatcher(this);
    }

    public void Submit()
    {
        Renderer.Render();
        Backend.CommandsExecute();
        Resources.FreeAllocations();
        Statistic.Clear();
    }
}

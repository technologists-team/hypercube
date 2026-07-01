using Hypercube.Graphics.Core.Device.Modules;
using Hypercube.Graphics.Device;

namespace Hypercube.Graphics.Core.Device;

public sealed class GraphicDevice : IGraphicDeviceInternal
{
    public GraphicDeviceBackend Backend { get; }
    public GraphicDeviceStatistic Statistic { get; }
    public GraphicDeviceResources Resources { get; }

    private bool _frame;
    
    public GraphicDevice(in GraphicsDeviceSettings settings)
    {
        // Modules
        Backend = new GraphicDeviceBackend(this, settings);
        Statistic = new GraphicDeviceStatistic(this);
        Resources = new GraphicDeviceResources(this);
    }

    public void FrameStart()
    {
        if (_frame)
            return;
        
        OnFrameStart();
        
        _frame = true;
    }

    public void FrameEnd()
    {
        if (!_frame)
            return;
        
        OnFrameEnd();
        
        _frame = false;
    }

    private void OnFrameStart()
    {
    }

    private void OnFrameEnd()
    {
        Backend.CommandsExecute();
        Resources.FreeAllocations();
        Statistic.Clear();
    }
}

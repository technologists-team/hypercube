using Hypercube.Graphics.Backend;
using Hypercube.Graphics.Device;
using Hypercube.Utilities.Commander;

namespace Hypercube.Graphics.Core.Device;

public sealed class GraphicDevice : IGraphicDevice
{
    private readonly IBackend _backend;
    private readonly IUnsafeCommandBuffer _commandBuffer;
    
    private readonly GraphicDeviceResources _resources;

    private bool _frame;
    
    public GraphicDevice(in GraphicsDeviceSettings settings)
    {
        _resources = new GraphicDeviceResources(this);
        
        _backend = BackendFactory.Create(settings.Backend);
        _backend.Initialize(settings);
        
        _commandBuffer = new UnsafeCommandBuffer();
    }

    public void RaiseCommand<T>(T command, LowCommandType type) where T : unmanaged
    {
        _commandBuffer.Push(command, (ushort) type);
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
        _backend.FrameStart();
        _commandBuffer.Reset();   
    }

    private void OnFrameEnd()
    {
        _backend.ExecuteCommands(_commandBuffer);
        _backend.EndFrame();
        
        _resources.FreeAllocations();
    }
}

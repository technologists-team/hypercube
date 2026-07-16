using Hypercube.Graphics.Backend;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Types;
using Hypercube.Utilities.Commander;

namespace Hypercube.Graphics.Core.Device.Modules;

public sealed class GraphicDeviceBackend : GraphicDeviceModule
{
    private readonly IBackend _backend;
    private readonly IUnsafeCommandBuffer _commandBuffer;

    public GraphicDeviceBackend(GraphicDevice device, GraphicsDeviceSettings settings) : base(device)
    {
        // We granted not null in GraphicDevice
        _backend = BackendFactory.Create(device.BackendType);
        _backend.Initialize(settings);
        
        _commandBuffer = new UnsafeCommandBuffer();
    }

    public void CommandPush<T>(T command, LowCommandType type) where T : unmanaged
    {
        _commandBuffer.Push(command, (ushort) type);
    }
    
    public void CommandsExecute()
    {
        _backend.ExecuteCommands(_commandBuffer);
        _commandBuffer.Reset();
    }
}

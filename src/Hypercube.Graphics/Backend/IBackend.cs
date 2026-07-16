using Hypercube.Graphics.Device;
using Hypercube.Utilities.Commander;

namespace Hypercube.Graphics.Backend;

public interface IBackend
{
    void Initialize(in GraphicsDeviceSettings settings);
    
    void Terminate();

    void ExecuteCommands(IUnsafeCommandBuffer commandBuffer);
}

using Hypercube.Graphics.Device;
using Hypercube.Utilities.Commander;

namespace Hypercube.Graphics.Backend;

public interface IBackend
{
    void ExecuteCommands(IUnsafeCommandBuffer commandBuffer);
    void EndFrame();
    void FrameStart();
    void Initialize(in GraphicsDeviceSettings settings);
}
using Hypercube.Graphics.Core.Attributes;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Types;
using Hypercube.Utilities.Commander;

namespace Hypercube.Graphics.Backend.Realisation.Vulkan;

[Backend(BackendType.Vulkan)]
public sealed class VulkanBackend : IBackend
{
    public void Initialize(in GraphicsDeviceSettings settings)
    {
        throw new NotImplementedException();
    }

    public void Terminate()
    {
        throw new NotImplementedException();
    }

    public void ExecuteCommands(IUnsafeCommandBuffer commandBuffer)
    {
        throw new NotImplementedException();
    }
}
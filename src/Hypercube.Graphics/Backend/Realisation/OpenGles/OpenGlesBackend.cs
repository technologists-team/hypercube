using Hypercube.Graphics.Core.Attributes;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Types;
using Hypercube.Utilities.Commander;

namespace Hypercube.Graphics.Backend.Realisation.OpenGles;

[Backend(BackendType.OpenGles)]
public sealed class OpenGlesBackend : IBackend
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
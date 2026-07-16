using Hypercube.Graphics.Resources.Shaders;

namespace Hypercube.Graphics.Backend.Commands.Shader;

public unsafe struct LowCommandCreateShader
{
    public ShaderBackendHandle Handle;
    public void* Data;
    public int DataSize;
}

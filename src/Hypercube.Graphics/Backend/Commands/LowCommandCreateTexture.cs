using Hypercube.Graphics.Resources.Textures;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Backend.Commands;

public unsafe struct LowCommandCreateTexture
{
    public TextureBackendHandle Handle;

    public Vector2i Size;
    
    public void* Data;
    public int DataSize;
}
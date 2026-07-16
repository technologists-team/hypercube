using Hypercube.Graphics.Resources.Textures;
using Hypercube.Graphics.Resources.Textures.Types;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Backend.Commands.Texture;

public unsafe struct LowCommandCreateTexture
{
    public TextureBackendHandle Handle;
    public TextureType Type;
    public TextureFormat Format;
    public TexturePixelFormat PixelFormat;
    
    public Vector2i Size;

    public void* Data;
    public int DataSize;
}

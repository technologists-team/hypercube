using Hypercube.Graphics.Resources;
using Hypercube.Graphics.Resources.Textures;

namespace Hypercube.Graphics;

public interface IResourceContext
{
    TextureId CreateTexture(Stream stream);
    TextureId CreateTexture(byte[] data);
    TextureId CreateTexture(ReadOnlyMemory<byte> data);
}

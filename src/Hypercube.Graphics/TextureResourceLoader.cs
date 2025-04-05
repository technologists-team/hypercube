using Hypercube.Resources;
using Hypercube.Resources.FileSystems;
using Hypercube.Resources.Loaders;

namespace Hypercube.Graphics;

public sealed class TextureResourceLoader : ResourceLoader<Texture2D>
{
    public override string[] Extensions =>
    [
        "png", "jpg", "jpeg", "bmp", "tga"
    ];

    public override bool CanLoad(ResourcePath path, IFileSystem fileSystem)
    {
        throw new NotImplementedException();
    }

    public override Texture2D Load(ResourcePath path, IFileSystem fileSystem)
    {
        throw new NotImplementedException();
    }
}
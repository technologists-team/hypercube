using Hypercube.Core.Resources;
using Hypercube.Core.Resources.FileSystems;
using Hypercube.Core.Resources.Loaders;
using Hypercube.Mathematics.Shapes;
using StbImageSharp;

namespace Hypercube.Core.Graphics.Resources;

public sealed class TextureResourceLoader : ResourceLoader<Texture>
{
    public override string[] Extensions =>
    [
        "png", "jpg", "jpeg", "bmp", "tga"
    ];

    public override bool CanLoad(ResourcePath path, IFileSystem fileSystem)
    {
        return true;
    }

    public override Texture Load(ResourcePath path, IFileSystem fileSystem)
    {
        using var stream = fileSystem.OpenRead(path);
        var result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        return new Texture(result.Data, new Vector2i(result.Width, result.Height), (int) ColorComponents.RedGreenBlueAlpha, Rect2.UV);
    }
}
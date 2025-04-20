using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources;
using Hypercube.Resources.FileSystems;
using Hypercube.Resources.Loaders;
using StbImageSharp;

namespace Hypercube.Graphics;

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
        return new Texture(new Vector2i(result.Width, result.Height), result.Data, Box2.UV);
    }
}
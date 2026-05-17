using Hypercube.Core.Graphics.Objects.Fonts;
using Hypercube.Core.Resources;
using Hypercube.Core.Resources.FileSystems;
using Hypercube.Core.Resources.Loaders;
using Hypercube.Mathematics.Shapes;
using StbImageSharp;

namespace Hypercube.Core.Graphics.Resources;

public class FontResourceLoader : ResourceLoader<Font>
{
    public const int DefaultSize = 16;
    
    public override string[] Extensions => ["ttf", "otf"];
    public override bool SupportLoadArgs => true;

    public override bool CanLoad(ResourcePath path, IFileSystem fileSystem)
    {
        return Extensions.Contains(path.Extension, StringComparer.OrdinalIgnoreCase);
    }

    public override Font Load(ResourcePath path, IFileSystem fileSystem)
    {
        return Load(path, DefaultSize, fileSystem);
    }

    public override Font Load(ResourcePath path, IFileSystem fileSystem, ResourceLoadArg[] args)
    {
        var size = DefaultSize;
        foreach (var arg in args)
        {
            if (arg is { Key: "size", Value: int value })
                size = value;
        }
        
        return Load(path, size, fileSystem);
    }

    private static Font Load(ResourcePath path, int size, IFileSystem fileSystem)
    {
        var fontData = GetData(fileSystem.OpenRead(path));
        var texture = CreateTexture(FontAtlasGenerator.Generate(fontData, size, out var info));
        return new Font(texture, info);
    }

    private static byte[] GetData(Stream stream)
    {
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }
    
    private static Texture CreateTexture(Stream stream)
    {
        var result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        return new Texture(result.Data, new Vector2i(result.Width, result.Height), (int) ColorComponents.RedGreenBlueAlpha, Rect2.UV);
    }
}

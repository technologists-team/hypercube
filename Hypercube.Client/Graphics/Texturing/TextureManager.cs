using Hypercube.Math.Vector;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureManager : ITextureManager
{
    public ITexture Create(string path)
    {
        using var stream = File.OpenRead(path);
        return Create(Image.Load<Rgba32>(stream));
    }

    public ITextureHandle CrateHandler(ITexture texture)
    {
        return new TextureHandle(texture);
    }
    
    public ITextureHandle CreateHandler(string path)
    {
        return CrateHandler(Create(path));
    }

    private ITexture Create(Image<Rgba32> image)
    {
        return new Texture((image.Width, image.Height), GenerateData(image));
    }

    private int[,] GenerateData(Image<Rgba32> image)
    {
        var size = new Vector2Int(image.Width, image.Height);
        var data = new int[image.Width, image.Height];

        for (var x = 0; x < size.X; x++)
        {
            for (var y = 0; y < size.Y; y++)
            {
                data[x, y] = ConvertRgba32ToInt(image[x, y]);
            }
        }

        return data;
    }

    private static int ConvertRgba32ToInt(Rgba32 rgba32)
    {
        return rgba32.R << 24 |
               rgba32.G << 16 |
               rgba32.B << 8 |
               rgba32.A;
    }
}
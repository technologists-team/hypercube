using Hypercube.Client.Graphics.Texturing.Settings;
using Hypercube.Dependencies;
using Hypercube.Graphics.Texturing;
using Hypercube.Graphics.Texturing.Settings;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources;
using Hypercube.Resources.Manager;
using StbImageSharp;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Texturing;

public sealed class TextureManager : ITextureManager
{
    [Dependency] private readonly IResourceLoader _resourceLoader = default!;

    public ITexture CreateBlank(Vector2i size, Color color)
    {
        var data = new byte[size.X * size.Y * 4];
        
        for (var x = 0; x < size.X; x++)
        {
            for (var y = 0; y < size.Y; y++)
            {
                data[x + y + 0] = color.ByteR;
                data[x + y + 1] = color.ByteG;
                data[x + y + 2] = color.ByteB;
                data[x + y + 3] = color.ByteA;
            }
        }

        return new Texture(string.Empty, size, data);
    }
    
    public ITexture CreateTexture(ResourcePath path)
    {
        using var stream = _resourceLoader.ReadFileContent(path);
        var result = ImageResult.FromStream(stream);
        var texture = new Texture(path, (result.Width, result.Height), result.Data);
        return texture;
    }
    
    public ITextureHandle CreateTextureHandle(ITexture texture)
    {
        return CreateTextureHandle(texture, new TextureCreationSettings());
    }
    
    public ITextureHandle CreateTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return new TextureHandle(texture, settings);
    }
    
    public ITextureHandle CreateTextureHandle(ResourcePath path)
    {
        return CreateTextureHandle(path, new TextureCreationSettings());
    }
    
    public ITextureHandle CreateTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        return new TextureHandle(CreateTexture(path), settings);
    }
}
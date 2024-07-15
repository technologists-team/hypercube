using Hypercube.Client.Graphics.Texturing.Settings;
using Hypercube.Math;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureManager
{
    ITexture CreateBlank(Vector2Int size, Color color);
    ITexture CreateTexture(ResourcePath path);
    ITextureHandle CreateTextureHandle(ITexture texture);
    ITextureHandle CreateTextureHandle(ITexture texture, ITextureCreationSettings settings);
    ITextureHandle CreateTextureHandle(ResourcePath path);
    ITextureHandle CreateTextureHandle(ResourcePath path, ITextureCreationSettings settings);
}
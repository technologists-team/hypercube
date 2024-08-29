using Hypercube.Graphics.Texturing.Settings;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing;

[PublicAPI]
public interface ITextureManager
{
    ITexture CreateBlank(Vector2i size, Color color);
    ITexture CreateTexture(ResourcePath path);
    
    ITextureHandle CreateTextureHandle(ITexture texture);
    ITextureHandle CreateTextureHandle(ITexture texture, ITextureCreationSettings settings);
    ITextureHandle CreateTextureHandle(ResourcePath path);
    ITextureHandle CreateTextureHandle(ResourcePath path, ITextureCreationSettings settings);
}
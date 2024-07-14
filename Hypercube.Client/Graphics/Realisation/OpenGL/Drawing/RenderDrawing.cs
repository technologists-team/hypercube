using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Dependency;
using Hypercube.Math;
using Hypercube.Math.Boxs;
using Hypercube.Math.Matrixs;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Resources.Caching;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Drawing;

public sealed class RenderDrawing : IRenderDrawing
{
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly IResourceCacher _resourceCacher = default!;
    
    public void DrawTexture(ITexture texture, Vector2 position)
    {
        DrawTexture(texture, texture.QuadCrateTranslated(position), Box2.UV);
    }

    public void DrawTexture(ITexture texture, Vector2 position, Color color)
    {
        DrawTexture(texture, texture.QuadCrateTranslated(position), Box2.UV, color);
    }

    public void DrawTexture(ITextureHandle texture, Vector2 position, Color color)
    {
        DrawTexture(texture, texture.Texture.QuadCrateTranslated(position), Box2.UV, color);
    }
    
    public void DrawTexture(ITexture texture, Box2 quad, Box2 uv)
    {
        DrawTexture(texture, quad, uv, Color.White);
    }

    public void DrawTexture(ITexture texture, Box2 quad, Box2 uv, Color color)
    {
        var handle = _resourceCacher.GetResource<TextureResource>(texture.Path).Texture;
        _renderer.DrawTexture(handle, quad, uv, color);
    }
    
    public void DrawTexture(ITextureHandle texture, Vector2 position, Color color, Matrix4X4 model)
    {
        _renderer.DrawTexture(texture, texture.Texture.QuadCrateTranslated(position), Box2.UV, color, model);
    }
    
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color)
    {
        _renderer.DrawTexture(texture, quad, uv, color);
    }
    
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model)
    {
        _renderer.DrawTexture(texture, quad, uv, color, model);
    }
}
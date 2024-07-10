using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Drawing;

public sealed class RenderDrawing : IRenderDrawing
{
    [Dependency] private readonly IRenderer _renderer = default!;
    
    public void DrawTexture(ITexture texture, Vector2 position)
    {
        DrawTexture(texture, texture.QuadCrateTranslated(position), new Box2(0.0f, 1.0f, 1.0f, 0.0f));
    }

    public void DrawTexture(ITexture texture, Vector2 position, Color color)
    {
        DrawTexture(texture, texture.QuadCrateTranslated(position), new Box2(0.0f, 1.0f, 1.0f, 0.0f), color);
    }

    public void DrawTexture(ITexture texture, Box2 quad, Box2 uv)
    {
        DrawTexture(texture, quad, uv, Color.White);
    }

    public void DrawTexture(ITexture texture, Box2 quad, Box2 uv, Color color)
    {
        _renderer.DrawTexture(new TextureHandle(texture), quad, uv, color);
    }
}
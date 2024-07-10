using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Drawing;

public interface IRenderDrawing
{
    void DrawTexture(ITexture texture, Vector2 position);
    void DrawTexture(ITexture texture, Vector2 position, Color color);
    void DrawTexture(ITexture texture, Box2 quad, Box2 uv);
    void DrawTexture(ITexture texture, Box2 quad, Box2 uv, Color color);
}
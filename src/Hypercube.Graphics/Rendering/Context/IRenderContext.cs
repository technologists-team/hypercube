using Hypercube.Graphics.Rendering.Api;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Rendering.Context;

public interface IRenderContext
{
    void Init(IRenderingApi api);
    void DrawRectangle(Box2 box, Color color, bool outline = false);
    void DrawTexture(Texture texture, Vector2 position, Color color);
}
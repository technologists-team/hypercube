using Hypercube.Graphics.Resources;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics;

/// <summary>
/// The primary rendering method provides all the necessary context for dispatching high-level rendering commands.
/// </summary>
public interface IRenderContext
{
    void DrawPoint(Vector2 position);
    void DrawPoint(Vector2 position, Color color);
    
    void DrawLine(Vector2 position, Vector2 positionTarget);
    void DrawLine(Vector2 position, Vector2 positionTarget, Color color);
    void DrawLine(Vector2 position, Vector2 positionTarget, float thickness);
    void DrawLine(Vector2 position, Vector2 positionTarget, float thickness, Color color);
    
    void DrawCircle(Vector2 position, float radius, bool outline);
    void DrawCircle(Vector2 position, float radius, bool outline, Color color);

    void DrawTriangle(Vector2 position1, Vector2 position2, Vector2 position3, bool outline);
    void DrawTriangle(Vector2 position1, Vector2 position2, Vector2 position3, bool outline, Color color);
    void DrawTriangle(Vector2 position1, Vector2 position2, Vector2 position3, bool outline, Color color1, Color color2, Color color3);
    
    void DrawRectangle(Vector2 position, Vector2 positionTarget, bool outline);
    void DrawRectangle(Vector2 position, Vector2 positionTarget, bool outline, Color color);
    void DrawRectangle(Vector2 position, Vector2 positionTarget, bool outline, Color color1, Color color2, Color color3, Color color4);
    
    void DrawTexture(TextureId texture, Vector2 position);
    void DrawTexture(TextureId texture, Vector2 position, Vector2 scale, Angle rotation, Color color);
}

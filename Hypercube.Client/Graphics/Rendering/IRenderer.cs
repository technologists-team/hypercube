using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Graphics.Windowing;
using Hypercube.Math;
using Hypercube.Math.Matrices;
using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Client.Graphics.Rendering;

public interface IRenderer
{
    WindowHandle MainWindow { get; }
    IReadOnlyDictionary<WindowId, WindowHandle> Windows { get; }
    
    void EnterWindowLoop();
    void TerminateWindowLoop();

    WindowHandle CreateWindow(WindowCreateSettings settings);
    void DestroyWindow(WindowHandle handle);
    void CloseWindow(WindowHandle handle);

    void AddMonitor(MonitorRegistration monitor);
    
    void OnFocusChanged(WindowHandle window, bool focused);
    
    // Drawing
    void DrawPoint(Vector2 vector, Color color);
    void DrawPoint(Vector2 vector, Color color, Matrix3X3 model);
    void DrawPoint(Vector2 vector, Color color, Matrix4X4 model);
    void DrawLine(Vector2 pointA, Vector2 pointB, Color color);
    void DrawLine(Vector2 pointA, Vector2 pointB, Color color, Matrix3X3 model);
    void DrawLine(Vector2 pointA, Vector2 pointB, Color color, Matrix4X4 model);
    void DrawLine(Box2 box, Color color);
    void DrawLine(Box2 box, Color color, Matrix3X3 model);
    void DrawLine(Box2 box, Color color, Matrix4X4 model);
    void DrawCircle(Circle circle, Color color);
    void DrawCircle(Circle circle, Color color, Matrix3X3 model);
    void DrawCircle(Circle circle, Color color, Matrix4X4 model);
    void DrawRectangle(Box2 box, Color color, bool outline = false);
    void DrawRectangle(Box2 box, Color color, Matrix3X3 model, bool outline = false);
    void DrawRectangle(Box2 box, Color color, Matrix4X4 model, bool outline = false);
    void DrawPolygon(Vector2[] vertices, Color color, bool outline = false);
    void DrawPolygon(Vector2[] vertices, Color color, Matrix3X3 model, bool outline = false);
    void DrawPolygon(Vector2[] vertices, Color color, Matrix4X4 model, bool outline = false);
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color);
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model);
}
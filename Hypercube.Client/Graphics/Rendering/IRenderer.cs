using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Math;
using Hypercube.Math.Matrixs;
using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Client.Graphics.Rendering;

public interface IRenderer
{
    WindowRegistration MainWindow { get; }
    IReadOnlyDictionary<WindowId, WindowRegistration> Windows { get; }
    
    void EnterWindowLoop();
    void TerminateWindowLoop();

    WindowRegistration CreateWindow(WindowCreateSettings settings);
    void DestroyWindow(WindowRegistration registration);
    void CloseWindow(WindowRegistration registration);

    void AddMonitor(MonitorRegistration monitor);
    
    void OnFocusChanged(WindowRegistration window, bool focused);
    
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
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color);
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model);
}
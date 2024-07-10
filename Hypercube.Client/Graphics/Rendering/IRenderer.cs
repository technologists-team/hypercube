using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;

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
    void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color);
}
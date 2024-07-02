using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Shared.Timing;

namespace Hypercube.Client.Graphics;

public interface IRenderer
{
    WindowRegistration MainWindow { get; }
    IReadOnlyDictionary<WindowId, WindowRegistration> Windows { get; }

    event Action<WindowRegistration>? WindowClosed;
    event Action<WindowRegistration>? MainWindowClosed;
    
    void Startup();
    void StartupLoop();

    void EnterWindowLoop();
    void TerminateWindowLoop();

    WindowRegistration CreateWindow(WindowCreateSettings settings);
    void DestroyWindow(WindowRegistration registration);
    void CloseWindow(WindowRegistration registration);
    
    void FrameUpdate(object? sender, FrameEventArgs args);
    void RenderFrame(object? sender, FrameEventArgs args);

    void AddMonitor(MonitorRegistration monitor);
}
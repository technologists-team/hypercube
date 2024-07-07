using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Windows;

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
}
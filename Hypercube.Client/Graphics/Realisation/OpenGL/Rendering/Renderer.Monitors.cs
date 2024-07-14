using Hypercube.Client.Graphics.Monitors;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    private readonly Dictionary<MonitorId, MonitorRegistration> _monitors = new();
    
    public void AddMonitor(MonitorRegistration monitor)
    {
        _monitors.Add(monitor.Handle.Id, monitor);
        _logger.EngineInfo($"Register {monitor.Handle}");
    }
}
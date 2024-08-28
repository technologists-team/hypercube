using Hypercube.Graphics.Monitors;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    private readonly Dictionary<MonitorId, MonitorHandle> _monitors = new();
    
    public void AddMonitor(MonitorHandle monitor)
    {
        _monitors.Add(monitor.Id, monitor);
        _logger.EngineInfo($"Register {monitor}");
    }
}
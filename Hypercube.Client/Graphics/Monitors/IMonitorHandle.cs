using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Monitors;

public interface IMonitorHandle
{
    MonitorId Id { get; }
    string Name { get; }
    Vector2Int Size { get; }
    int RefreshRate { get; }
    VideoMode[] VideoModes { get; }
}
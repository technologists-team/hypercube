using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Monitors;

public class MonitorHandle(MonitorId id, string name, Vector2Int size, int refreshRate, VideoMode[] videoModes) : IMonitorHandle
{
    public MonitorId Id { get; } = id;
    public string Name { get; } = name;
    public Vector2Int Size { get; } = size;
    public int RefreshRate { get; } = refreshRate;
    public VideoMode[] VideoModes { get; } = videoModes;

    public MonitorHandle(int id, string name, int width, int height, int refreshRate, VideoMode[] videoModes) : this(new MonitorId(id), name, (width, height), refreshRate, videoModes)
    {
        
    }

    public override string ToString()
    {
        return $"MonitorHandle {Id.Value}, name: {Name}, size: {size}, rate: {refreshRate}, mode: ({videoModes.Length})";
    }
}
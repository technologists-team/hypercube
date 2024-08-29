using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Monitors;

[PublicAPI]
public class MonitorHandle
{
    public MonitorId Id { get; }
    public string Name { get; }
    public Vector2i Size { get; }
    public int RefreshRate { get; }
    public VideoMode[] VideoModes { get; }

    public MonitorHandle(MonitorId id, string name, Vector2i size, int refreshRate, VideoMode[] videoModes)
    {
        Id = id;
        Name = name;
        Size = size;
        RefreshRate = refreshRate;
        VideoModes = videoModes;
    }

    public MonitorHandle(MonitorId id, string name, int width, int height, int refreshRate, VideoMode[] videoModes)
    {
        Id = id;
        Name = name;
        Size = new Vector2i(width, height);
        RefreshRate = refreshRate;
        VideoModes = videoModes;
    }

    public override string ToString()
    {
        return $"MonitorHandle {Id.Value}, name: {Name}, size: {Size}, rate: {RefreshRate}, mode: ({VideoModes.Length})";
    }
}
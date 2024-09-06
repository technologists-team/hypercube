using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Monitors;

[PublicAPI]
public class MonitorHandle
{
    public nint Pointer { get; }
    public MonitorId Id { get; }
    public string Name { get; }
    public VideoMode VideoMode { get; }
    public VideoMode[] VideoModes { get; }

    public Vector2i Size => new(VideoMode.Width, VideoMode.Height);
    public int RefreshRate => VideoMode.RefreshRate;
    
    public MonitorHandle(nint pointer, MonitorId id, string name, VideoMode videoMode, VideoMode[] videoModes)
    {
        Pointer = pointer;
        Id = id;
        Name = name;
        VideoMode = videoMode;
        VideoModes = videoModes;
    }

    public override string ToString()
    {
        return $"{Name} [{Id.Value}]({Pointer}) mode: ({VideoMode}), count: {VideoModes.Length}";
    }
}
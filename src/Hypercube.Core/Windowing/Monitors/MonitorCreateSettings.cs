using Hypercube.Core.Windowing.Monitors.Data;

namespace Hypercube.Core.Windowing.Monitors;

[PublicAPI]
public readonly struct MonitorCreateSettings
{
    public string Name { get; init; }
    public bool Primary { get; init; }
    
    public Vector2i Position { get; init; }
    public Vector2i PhysicalSize { get; init; }
    public Vector2 Dpi { get; init; }
    
    public Vector2 ContentScale { get; init; }
    public WorkArea WorkArea { get; init; }
    
    public VideoMode CurrentVideoMode { get; init; }
    public IReadOnlyList<VideoMode> VideoModes { get; init; }

    public static Vector2 CalculateDpi(in VideoMode mode, Vector2i physicalSize)
    {
        return CalculateDpi(mode.Size, physicalSize);
    }
    
    public static Vector2 CalculateDpi(Vector2i size, Vector2i physicalSize)
    {
        const float mmPerInch = 25.4f;
        
        if (physicalSize.X <= 0 || physicalSize.Y <= 0)
            return Vector2.Zero;

        return size / (physicalSize / mmPerInch);
    }
}

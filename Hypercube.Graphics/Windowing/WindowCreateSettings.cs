using Hypercube.Graphics.Monitors;
using Hypercube.Graphics.Texturing;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

[PublicAPI]
public class WindowCreateSettings
{
    public int Width => Size.X;
    public int Height => Size.Y;
    
    public string Title { get; init; } = "Hypercube Window";
    public Vector2i Size { get; init; } = new(1280, 720);
    
    public ITexture[]? WindowImages { get; init; }
    public MonitorHandle? Monitor { get; init; }

    public bool Resizable { get; init; } = true;
    public bool TransparentFramebuffer { get; init; }
    public bool Decorated { get; init; } = true;
    public bool Visible { get; init; } = true;
    
    public int? RedBits { get; init; } = 8;
    public int? GreenBits { get; init; } = 8;
    public int? BlueBits { get; init; } = 8;
    public int? AlphaBits { get; init; } = 8;

    public int? DepthBits { get; init; } = 24;
    public int? StencilBits { get; init; } = 8;
}
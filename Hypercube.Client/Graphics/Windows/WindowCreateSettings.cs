using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Windows;

public class WindowCreateSettings
{
    public int Width => Size.X;
    public int Height => Size.Y;
    
    public string Title = "Hypercube Window";
    public Vector2Int Size = new(1280, 720);
    public ITexture[]? WindowImages = null;

    public IMonitorHandle? Monitor;

    public bool Resizable = true;
    public bool TransparentFramebuffer = false;
    public bool Decorated = true;
    
    public int? RedBits = 8;
    public int? GreenBits = 8;
    public int? BlueBits = 8;
    public int? AlphaBits = 8;

    public int? DepthBits = 24;
    public int? StencilBits = 8;
}
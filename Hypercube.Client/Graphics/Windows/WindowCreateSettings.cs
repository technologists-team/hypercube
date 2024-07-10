using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Windows;

public class WindowCreateSettings
{
    public int Width => Size.X;
    public int Height => Size.Y;

    public bool NoTitleBar => Styles.HasFlag(WindowStyles.NoTitleBar);
    public bool NoTitleOptions => Styles.HasFlag(WindowStyles.NoTitleOptions);
    
    public string Title = "Hypercube Window";
    public Vector2Int Size = new(1280, 720);
    public WindowStyles Styles;
    public Version Version;
    public ITexture[]? WindowImages = null;

    public IMonitorHandle? Monitor;
    
    public int? RedBits = 8;
    public int? GreenBits = 8;
    public int? BlueBits = 8;
    public int? AlphaBits = 8;

    public int? DepthBits = 24;
    public int? StencilBits = 8;
}
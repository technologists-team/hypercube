using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Monitors;
using Hypercube.Windowing.Types;
using JetBrains.Annotations;

namespace Hypercube.Windowing.Windows;

[PublicAPI]
public readonly struct WindowCreateSettings()
{
    public string Title { get; init; } = "Hypercube";
    public Vector2i Size { get; init; } = new(600, 300);
    public Icon Icon { get; init; }
    
    public bool FullScreen { get; init; } = false;
    public bool Resizable { get; init; } = true;
    public bool Visible { get; init; } = true;
    public bool Decorated { get; init; } = true;
    public bool TransparentFramebuffer { get; init; } = false;
    public bool Floating { get; init; } = false;
    
    public IWindow? WindowContextShare { get; init; }
    public IMonitor? MonitorContextShare { get; init; }
    
    public WindowContextVersion ContextVersion { get; init; }
    public WindowContextType ContextType { get; init; }
}

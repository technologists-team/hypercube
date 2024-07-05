using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Windows;

public abstract class WindowRegistration
{   
    public bool IsDisposed;

    public WindowId Id;
    public WindowHandle Handle;
    public WindowHandle? Owner;
    public bool DisposeOnClose;
    
    public float Ratio { get; init; }
    
    public Vector2Int Size { get; init; }
    public Vector2Int FramebufferSize { get; init; }
}
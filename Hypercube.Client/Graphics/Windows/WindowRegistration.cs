using Hypercube.Math.Vectors;

namespace Hypercube.Client.Graphics.Windows;

public abstract class WindowRegistration
{   
    public bool IsDisposed;

    public WindowId Id;
    public WindowHandle Handle;
    public WindowHandle? Owner;
    public bool DisposeOnClose;
    
    public float Ratio { get; init; }
    
    public Vector2Int Size { get; set; }
    public Vector2Int FramebufferSize { get; init; }

    public void SetSize(int width, int height)
    {
        Size = new Vector2Int(width, height);
    }
} 
namespace Hypercube.Client.Graphics.Windows;

public abstract class WindowRegistration
{   
    public bool IsDisposed;

    public WindowId Id;
    public WindowHandle Handle;
    public WindowHandle? Owner;
    public bool DisposeOnClose;
}
using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Backend.Handler;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Core.Windows;

public sealed class Window : IWindow, IDisposable
{
    public event RouterWindowCloseHandler? OnClose;

    public Vector2i Size { get; private set; }
    public Vector2i FramebufferSize { get; private set; }
    public Vector2i Position { get; private set; }
    public bool Focus { get; private set; }
    
    private readonly WindowHandle _handle;
    private readonly BackendHandler _handler;
    private readonly WindowEventRouter _router;

    public Window(WindowHandle handle, BackendHandler handler)
    {
        _handle = handle;
        _handler = handler;
        
        _router = new WindowEventRouter(_handle, _handler);
        _router.OnClose += CloseCallback;
        _router.OnSize += SizeCallback;
        _router.OnFramebufferSize += FramebufferSizeCallback;
        _router.OnPosition += PositionCallback;
        _router.OnFocus += FocusCallback;
    }

    public void Dispose()
    {
        _router.OnClose -= CloseCallback;
        _router.OnSize -= SizeCallback;
        _router.OnFramebufferSize -= FramebufferSizeCallback;
        _router.OnPosition -= PositionCallback;
        _router.OnFocus -= FocusCallback;
    }

    public void MakeContextCurrent()
    {
        _handler.MakeContextCurrent(_handle);
    }

    public void SwapBuffers()
    {
        _handler.SwapBuffers(_handle);
    }

    public void Destroy()
    {
        _handler.WindowDestroy(_handle);
    }

    public void SetIcon(Icon icon)
    {
        _handler.SetIcon(_handle, icon);
    }

    public void SetPosition(Vector2i position)
    {
        _handler.SetPosition(_handle, position);
    }

    public void SetSize(Vector2i size)
    {
        _handler.SetSize(_handle, size);
    }

    public void SetTitle(string title)
    {
        _handler.SetTitle(_handle, title);
    }

    #region Event callbacks

    private void CloseCallback()
    {
        OnClose?.Invoke();
    }

    private void SizeCallback(Vector2i size)
    {
        Size = size;
    }

    private void FramebufferSizeCallback(Vector2i size)
    {
        FramebufferSize = size;
    }

    private void PositionCallback(Vector2i position)
    {
        Position = position;
    }

    private void FocusCallback(bool focused)
    {
        Focus = focused;
    }

    #endregion
}

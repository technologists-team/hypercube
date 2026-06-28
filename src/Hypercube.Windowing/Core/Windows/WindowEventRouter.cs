using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Backend.Handler;

namespace Hypercube.Windowing.Core.Windows;

public sealed class WindowEventRouter : IDisposable
{
    private readonly WindowHandle _handle;
    private readonly BackendHandler _backend;

    public event RouterWindowCloseHandler? OnClose;
    public event RouterWindowSizeHandler? OnSize;
    public event RouterWindowFramebufferSizeHandler? OnFramebufferSize;
    public event RouterWindowPositionHandler? OnPosition;
    public event RouterWindowFocusHandler? OnFocus;

    public WindowEventRouter(WindowHandle handle, BackendHandler backend)
    {
        _handle = handle;
        _backend = backend;

        _backend.OnWindowClose += HandleClose;
        _backend.OnWindowSize += HandleSize;
        _backend.OnWindowFramebufferSize += HandleFramebufferSize;
        _backend.OnWindowPosition += HandlePosition;
        _backend.OnWindowFocus += HandleFocus;
    }

    public void Dispose()
    {
        _backend.OnWindowSize -= HandleSize;
        _backend.OnWindowFramebufferSize -= HandleFramebufferSize;
        _backend.OnWindowPosition -= HandlePosition;
        _backend.OnWindowFocus -= HandleFocus;
    }

    private void HandleClose(WindowHandle handle)
    {
        if (!Filter(handle))
            return;

        OnClose?.Invoke();
    }

    private void HandleSize(WindowHandle handle, Vector2i size)
    {
        if (!Filter(handle))
            return;

        OnSize?.Invoke(size);
    }

    private void HandleFramebufferSize(WindowHandle handle, Vector2i size)
    {
        if (!Filter(handle))
            return;
        
        OnFramebufferSize?.Invoke(size);
    }

    private void HandlePosition(WindowHandle handle, Vector2i position)
    {
        if (!Filter(handle))
            return;

        OnPosition?.Invoke(position);
    }

    private void HandleFocus(WindowHandle handle, bool focused)
    {
        if (!Filter(handle))
            return;

        OnFocus?.Invoke(focused);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Filter(WindowHandle handle) => handle == _handle;
}
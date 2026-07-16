using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Backend.Events;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Wrappers;

public sealed class BackendListener : IDisposable
{
    public event Action<IEvent>? OnEvent;
    
    private readonly IBackend _backend;

    public BackendListener(IBackend backend)
    {
        _backend = backend;
        
        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _backend.OnWindowClose += HandleWindowClose;
        _backend.OnWindowFocus += HandleWindowFocus;
        _backend.OnWindowPosition += HandleWindowPosition;
        _backend.OnWindowSize += HandleWindowSize;
    }

    private void Unsubscribe()
    {
        _backend.OnWindowClose -= HandleWindowClose;
        _backend.OnWindowFocus -= HandleWindowFocus;
        _backend.OnWindowPosition -= HandleWindowPosition;
        _backend.OnWindowSize -= HandleWindowSize;
    }
    
    private void HandleWindowClose(WindowHandle window)
    {
        OnEvent?.Invoke(new EventWindowClose(window));
    }

    private void HandleWindowFocus(WindowHandle window, bool focused)
    {
        OnEvent?.Invoke(new EventWindowFocus(window, focused));
    }

    private void HandleWindowPosition(WindowHandle window, Vector2i position)
    {
        OnEvent?.Invoke(new EventWindowPosition(window, position));
    }

    private void HandleWindowSize(WindowHandle window, Vector2i size)
    {
        OnEvent?.Invoke(new EventWindowSize(window, size));
    }
}

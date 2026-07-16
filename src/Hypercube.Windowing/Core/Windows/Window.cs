using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Backend.Processors;
using Hypercube.Windowing.Core.Device;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Core.Windows;

/// <inheritdoc/>
public sealed partial class Window : IWindow
{
    /// <inheritdoc/>
    public event Action? OnClose;
    
    /// <inheritdoc/>
    public event Action<Vector2i>? OnSize;

    /// <inheritdoc/>
    public WindowHandle Handle { get; }
    
    /// <inheritdoc/>
    public Vector2i Size { get; private set; }
    
    /// <inheritdoc/>
    public Vector2i FramebufferSize { get; private set; }
    
    /// <inheritdoc/>
    public Vector2i Position { get; private set; }
    
    /// <inheritdoc/>
    public bool Focus { get; private set; }

    private readonly WindowingDevice _device;
    private readonly WindowingBackendProcessor _processor;

    private bool _disposed; 
    
    public Window(WindowHandle handle, WindowingDevice device, WindowingBackendProcessor processor)
    {
        Handle = handle;
        
        _device = device;
        _processor = processor;
        
        _processor.Raiser.OnWindowClose += CloseCallback;
        _processor.Raiser.OnWindowSize += SizeCallback;
        _processor.Raiser.OnWindowFramebufferSize += FramebufferSizeCallback;
        _processor.Raiser.OnWindowPosition += PositionCallback;
        _processor.Raiser.OnWindowFocus += FocusCallback;
    }

    /// <inheritdoc/>
    public void Show()
    {
        _processor.Executor.WindowShow(Handle);
    }

    /// <inheritdoc/>
    public void Hide()
    {
        _processor.Executor.WindowHide(Handle);
    }

    /// <inheritdoc/>
    public void Minimize()
    {
        _processor.Executor.WindowMinimize(Handle);
    }

    /// <inheritdoc/>
    public void Maximize()
    {
        _processor.Executor.WindowMaximize(Handle);
    }

    /// <inheritdoc/>
    public void Restore()
    {
        _processor.Executor.WindowRestore(Handle);
    }

    /// <inheritdoc/>
    public void SetIcon(Icon icon)
    {
        _processor.Executor.WindowSetIcon(Handle, icon);
    }

    public void SetIcons(Icon[] icons)
    {
        _processor.Executor.WindowSetIcons(Handle, icons);
    }
    
    /// <inheritdoc/>
    public void SetPosition(Vector2i position)
    {
        _processor.Executor.WindowSetPosition(Handle, position);
    }

    /// <inheritdoc/>
    public void SetSize(Vector2i size)
    {
        _processor.Executor.WindowSetSize(Handle, size);
    }

    /// <inheritdoc/>
    public void SetTitle(string title)
    {
        _processor.Executor.WindowSetTitle(Handle, title);
    }

    /// <inheritdoc/>
    public void SetContext()
    {
        _processor.Backend.WindowSetContext(Handle);
    }

    /// <inheritdoc/>
    public void SwapBuffers()
    {
        _processor.Backend.WindowSwapBuffers(Handle);
    }

    /// <inheritdoc/>
    public void Destroy()
    {
        Dispose();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
            return;
        
        _processor.Raiser.OnWindowClose -= CloseCallback;
        _processor.Raiser.OnWindowSize -= SizeCallback;
        _processor.Raiser.OnWindowFramebufferSize -= FramebufferSizeCallback;
        _processor.Raiser.OnWindowPosition -= PositionCallback;
        _processor.Raiser.OnWindowFocus -= FocusCallback;
        
        _device.WidowRemove(this);
        
        _processor.Executor.WindowDestroy(Handle);
        
        _disposed = true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WindowFilter(WindowHandle handle) => Handle ==  handle;
}

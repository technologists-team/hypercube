using System.Runtime.CompilerServices;
using Hypercube.Core.Graphics.Objects.Texturing;
using Hypercube.Core.Windowing.Api;

namespace Hypercube.Core.Windowing.Windows;

/// <inheritdoc/>
[EngineInternal]
public sealed partial class Window : IWindow
{
    #region Public Events

    /// <inheritdoc/>
    public event Action<string>? OnChangedTitle;
    
    /// <inheritdoc/>
    public event Action<Vector2i>? OnChangedPosition;
    
    /// <inheritdoc/>
    public event Action<Vector2i>? OnChangedSize;

    public event Action<Vector2i>? OnChangedFramebufferSize;
    
    public event Action? OnClose;

    public event Action? OnDisposed;

    #endregion

    private bool _disposed;
    private bool _destroyed;

    /// <inheritdoc/>
    public IWindowingApi Api { get; }
    
    /// <inheritdoc/>
    public WindowHandle Handle => _destroyed ? throw new InvalidOperationException() : field;

    /// <inheritdoc/>
    public WindowHandle Context => Api.Context;

    /// <inheritdoc/>
    public WindowingApi Type => Api.Type;

    /// <inheritdoc/>
    public string Title
    {
        get => _cachedTitle;
        set => Api.WindowSetTitle(Handle, value);
    }

    /// <inheritdoc/>
    public Vector2i Position
    {
        get => _cachedPosition;
        set => Api.WindowSetPosition(Handle, value);
    }

    /// <inheritdoc/>
    public Vector2i Size
    {
        get => _cachedSize;
        set => Api.WindowSetSize(Handle, value);
    }

    public Vector2i FramebufferSize
    {
        get => _cachedFramebufferSize;
        set => Api.WindowSetFramebufferSize(Handle, value);
    }

    public IImage Icon
    {
        set => Api.WindowSetIcon(Handle, value);
    }
    
    public bool IsMain { get; set; }

    private Window(IWindowingApi api, WindowHandle handle)
    {
        Api = api;
        Handle = handle;
    }
    
    public Window(IWindowingApi api, WindowHandle handle, WindowCreateSettings settings) : this(api, handle)
    {
        _cachedTitle = settings.Title;
        _cachedSize = settings.Size;

        Api.OnWindowTitle += OnApiTitle;
        Api.OnWindowPosition += OnApiPosition;
        Api.OnWindowSize += OnApiSize;
        Api.OnWindowFramebufferSize += OnApiFramebufferSize;
        Api.OnWindowClose += OnApiClose;
    }

    ~Window()
    {
        Dispose(false);
    }

    /// <inheritdoc/>
    public void MakeCurrent()
    {
        Api.Context = Handle;
    }

    /// <inheritdoc/>
    public void SwapBuffers()
    {
        Api.WindowSwapBuffers(Handle);
    }

    /// <inheritdoc/>
    public nint GetProcAddress(string name)
    {
        return Api.GetProcAddress(name);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            // Managed
            Api.OnWindowTitle -= OnApiTitle;
            Api.OnWindowPosition -= OnApiPosition;
            Api.OnWindowSize -= OnApiSize;
        }
        
        // Unmanaged
        Destroy();
        
        OnDisposed?.Invoke();
        _disposed = true;
    }

    public bool Equals(IWindow? other)
    {
        return other is not null && Handle == other.Handle;
    }

    public override bool Equals(object? obj)
    {
        return obj is Window window && Equals(window);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Handle);
    }

    public override string ToString()
    {
        return $"{Handle} ({Enum.GetName(Type)})";
    }

    private void OnApiTitle(WindowHandle window, string title)
    {
        if (window != Handle)
            return;

        _cachedTitle = title;
        OnChangedTitle?.Invoke(title);
    }

    private void OnApiPosition(WindowHandle window, Vector2i position)
    {
        if (window != Handle)
            return;

        _cachedPosition = position;
        OnChangedPosition?.Invoke(position);
    }

    private void OnApiSize(WindowHandle window, Vector2i size)
    {
       if (window != Handle)
           return;

       _cachedSize = size;
       OnChangedSize?.Invoke(size);
    }

    private void OnApiFramebufferSize(WindowHandle window, Vector2i size)
    {
        if (window != Handle)
            return;

        _cachedFramebufferSize = size;
        OnChangedFramebufferSize?.Invoke(size);
    }

    private void OnApiClose(WindowHandle window)
    {
        if (window != Handle)
            return;
        
        OnClose?.Invoke();
    }

    private void Destroy()
    {
        Api.WindowDestroy(Handle);
        _destroyed = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Window left, Window right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Window left, Window right) => !(left == right);
}

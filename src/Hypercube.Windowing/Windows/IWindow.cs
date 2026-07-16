using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Windows;

/// <summary>
/// Defines the contract for a window, providing properties and methods to manage its lifecycle, 
/// appearance, position, and underlying rendering context.
/// </summary>
public interface IWindow : IDisposable
{
    /// <summary>
    /// Occurs when the user or system requests to close the window.
    /// </summary>
    event Action? OnClose;
    
    /// <summary>
    /// Occurs when the size of the window's client area changes.
    /// </summary>
    event Action<Vector2i>? OnSize;
    
    /// <summary>
    /// Gets the native handle of the window.
    /// </summary>
    WindowHandle Handle { get; }
    
    /// <summary>
    /// Gets the size of the window's client area in screen coordinates.
    /// </summary>
    Vector2i Size { get; }
    
    /// <summary>
    /// Gets the size of the underlying framebuffer in pixels. 
    /// This may differ from <see cref="Size"/> on high-DPI (Retina) displays.
    /// </summary>
    Vector2i FramebufferSize { get; }
    
    /// <summary>
    /// Gets the position of the window's upper-left corner in screen coordinates.
    /// </summary>
    Vector2i Position { get; }
    
    /// <summary>
    /// Gets a value indicating whether the window currently has input focus.
    /// </summary>
    bool Focus { get; }
    
    /// <summary>
    /// Makes the window visible and brings it to the front.
    /// </summary>
    void Show();
    
    /// <summary>
    /// Hides the window.
    /// </summary>
    void Hide();
    
    /// <summary>
    /// Minimizes (iconifies) the window.
    /// </summary>
    void Minimize();
        
    /// <summary>
    /// Maximizes the window.
    /// </summary>
    void Maximize();
    
    /// <summary>
    /// Restores the window from a minimized or maximized state to its normal size.
    /// </summary>
    void Restore();
    
    /// <summary>
    /// Sets the icon for the window.
    /// </summary>
    /// <param name="icon">The icon to set.</param>
    void SetIcon(Icon icon);

    void SetIcons(Icon[] icons);
    
    /// <summary>
    /// Sets the position of the window on the screen.
    /// </summary>
    /// <param name="position">The new position of the window's upper-left corner.</param>
    void SetPosition(Vector2i position);
    
    /// <summary>
    /// Sets the size of the window's client area.
    /// </summary>
    /// <param name="size">The new size of the window.</param>
    void SetSize(Vector2i size);
    
    /// <summary>
    /// Sets the title of the window.
    /// </summary>
    /// <param name="title">The new title string.</param>
    void SetTitle(string title);

    /// <summary>
    /// Makes this window's rendering context current on the calling thread.
    /// </summary>
    void SetContext();
    
    /// <summary>
    /// Swaps the front and back buffers of the window's rendering context, 
    /// presenting the rendered frame to the screen.
    /// </summary>
    void SwapBuffers();
    
    /// <summary>
    /// Explicitly destroys the window and releases its underlying resources. 
    /// This method serves as a semantic alias for <see cref="IDisposable.Dispose"/>.
    /// </summary>
    void Destroy();
}

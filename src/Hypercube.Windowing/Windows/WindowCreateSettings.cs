using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Monitors;
using JetBrains.Annotations;

namespace Hypercube.Windowing.Windows;

/// <summary>
/// Contains configuration settings for creating and initializing a new window.
/// </summary>
[PublicAPI]
public readonly struct WindowCreateSettings()
{
    /// <summary>
    /// Gets or sets the initial title of the window.
    /// </summary>
    public string Title { get; init; } = "Hypercube";
    
    /// <summary>
    /// Gets or sets the initial size of the window's client area in screen coordinates.
    /// </summary>
    public Vector2i Size { get; init; } = new(600, 300);
    
    /// <summary>
    /// Gets or sets the icon to be displayed in the window's title bar and taskbar.
    /// </summary>
    public Icon Icon { get; init; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the window should be created in full-screen mode.
    /// </summary>
    public bool FullScreen { get; init; } = false;
    
    /// <summary>
    /// Gets or sets a value indicating whether the window's size can be changed by the user.
    /// </summary>
    public bool Resizable { get; init; } = true;
    
    /// <summary>
    /// Gets or sets a value indicating whether the window should be initially visible upon creation.
    /// </summary>
    public bool Visible { get; init; } = true;
    
    /// <summary>
    /// Gets or sets a value indicating whether the window should have standard OS decorations (title bar, borders).
    /// </summary>
    public bool Decorated { get; init; } = true;
    
    /// <summary>
    /// Gets or sets a value indicating whether the window's framebuffer should support transparency.
    /// </summary>
    public bool TransparentFramebuffer { get; init; } = false;
    
    /// <summary>
    /// Gets or sets a value indicating whether the window should float above all other non-floating windows (always-on-top).
    /// </summary>
    public bool Floating { get; init; } = false;
    
    /// <summary>
    /// Gets or sets an existing window with which to share the rendering context. 
    /// Objects like textures and vertex buffers can be shared between contexts.
    /// </summary>
    public IWindow? WindowContextShare { get; init; }
    
    /// <summary>
    /// Gets or sets the monitor to associate with the window's rendering context, 
    /// typically used for full-screen applications or display-specific context configurations.
    /// </summary>
    public IMonitor? MonitorContextShare { get; init; }
    
    /// <summary>
    /// Gets or sets the requested version of the graphics context (e.g., OpenGL 4.6).
    /// </summary>
    public WindowContextVersion ContextVersion { get; init; }
    
    /// <summary>
    /// Gets or sets the type of the graphics context to create (e.g., InternalDesktop, InternalEmbedded).
    /// </summary>
    public WindowContextType ContextType { get; init; }
}

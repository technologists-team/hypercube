using System.Text;
using Hypercube.Core.Graphics.Objects.Texturing;
using Hypercube.Core.Windowing.Api.Settings;
using Hypercube.Core.Windowing.Monitors;

namespace Hypercube.Core.Windowing.Windows;

[PublicAPI]
public readonly struct WindowCreateSettings
{
    /// <summary>
    /// Graphics/API-specific settings used to initialize the rendering context
    /// (e.g. OpenGL, Vulkan, version, flags, etc.).
    /// </summary>
    public ApiSettings Api { get; init; }
    
    /// <summary>
    /// The title of the window.
    /// </summary>
    public string Title { get; init; } = "Default Window";

    /// <summary>
    /// The initial size of the window in pixels (width, height).
    /// Ignored in exclusive fullscreen mode depending on platform.
    /// </summary>
    public Vector2i Size { get; init; } = new(800, 600);

    /// <summary>
    /// Indicates whether the window should be created in fullscreen mode.
    /// When enabled, the window is typically attached to a monitor and may override <see cref="Size"/>.
    /// </summary>
    public bool FullScreen { get; init; } = false;

    /// <summary>
    /// Indicates whether the user can resize the window.
    /// Has no effect in fullscreen mode.
    /// </summary>
    public bool Resizable { get; init; } = true;

    /// <summary>
    /// Indicates whether the window is visible immediately after creation.
    /// If false, the window must be shown manually.
    /// </summary>
    public bool Visible { get; init; } = true;

    /// <summary>
    /// Indicates whether the window has standard OS decorations
    /// (title bar, borders, close/minimize/maximize buttons).
    /// </summary>
    public bool Decorated { get; init; } = true;

    /// <summary>
    /// Enables a transparent framebuffer.
    /// Useful for compositing or overlay-style windows.
    /// Platform and GPU support may vary.
    /// </summary>
    public bool TransparentFramebuffer { get; init; } = false;
    
    /// <summary>
    /// Indicates whether the window should stay above other windows (always-on-top).
    /// </summary>
    public bool Floating { get; init; } = false;
    
    /// <summary>
    /// Handle to another window whose rendering context will be shared with this one.
    /// Allows sharing GPU resources such as textures, buffers, etc.
    /// Use <see cref="nint.Zero"/> if no sharing is required.
    /// </summary>
    public WindowHandle ContextShare { get; init; } = WindowHandle.Zero;

    /// <summary>
    /// Handle to the monitor used for fullscreen mode.
    /// Must be set when <see cref="FullScreen"/> is true.
    /// Use <see cref="nint.Zero"/> for windowed mode.
    /// </summary>
    public MonitorHandle MonitorShare { get; init; } = MonitorHandle.Zero;

    /// <summary>
    /// Enables vertical synchronization (VSync).
    /// When enabled, buffer swapping is synchronized with the display refresh rate,
    /// reducing tearing at the cost of potential input latency.
    /// </summary>
    public bool VSync { get; init; } = false;
    
    public IImage? Icon { get; init; }
     
    /// <summary>
    /// Initializes a new instance of <see cref="WindowCreateSettings"/>.
    /// </summary>
    public WindowCreateSettings()
    {
    }
    
    public override string ToString()
    {
        return new StringBuilder(nameof(WindowCreateSettings))
            .Append(" { ")
            .Append($"{nameof(Title)} = \"{Title}\", ")
            .Append($"{nameof(Size)} = {Size}, ")
            .Append($"{nameof(FullScreen)} = {FullScreen}, ")
            .Append($"{nameof(VSync)} = {VSync}, ")
            .Append($"{nameof(Resizable)} = {Resizable}, ")
            .Append($"{nameof(Visible)} = {Visible}, ")
            .Append($"{nameof(Decorated)} = {Decorated}, ")
            .Append($"{nameof(Floating)} = {Floating}, ")
            .Append($"{nameof(TransparentFramebuffer)} = {TransparentFramebuffer}, ")
            .Append($"{nameof(Api)} = {Api}, ")
            .Append($"{nameof(ContextShare)} = 0x{ContextShare:x8}, ")
            .Append($"{nameof(MonitorShare)} = 0x{MonitorShare:x8}")
            .Append(" }")
            .ToString();
    }
}

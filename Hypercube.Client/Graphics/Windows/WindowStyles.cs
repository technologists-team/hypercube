namespace Hypercube.Client.Graphics.Windows;

/// <summary>
/// Specifies window styling options for an OS window.
/// </summary>
[Flags]
public enum WindowStyles
{
    /// <summary>
    /// No special styles set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Hide title buttons such as close and minimize.
    /// </summary>
    NoTitleOptions = 1 << 0,

    /// <summary>
    /// Completely hide the title bar
    /// </summary>
    NoTitleBar = 1 << 1,
}
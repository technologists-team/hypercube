namespace Hypercube.Windowing.Windows;

/// <summary>
/// Represents a specific version of a graphics or windowing context, 
/// defined by a major and minor version number (e.g., OpenGL 4.6).
/// </summary>
public readonly struct WindowContextVersion(int major, int minor)
{
    /// <summary>
    /// Gets the major version number.
    /// </summary>
    public int Major { get; init; } = major;

    /// <summary>
    /// Gets the minor version number.
    /// </summary>
    public int Minor { get; init; } = minor;
}

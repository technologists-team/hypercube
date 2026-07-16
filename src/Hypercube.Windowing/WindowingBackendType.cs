namespace Hypercube.Windowing;

/// <summary>
/// Specifies the underlying native windowing backend used by the windowing system.
/// </summary>
public enum WindowingBackendType : byte
{
    /// <summary>
    /// No backend is specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Automatically selects the most appropriate backend for the current operating system.
    /// </summary>
    Auto = 1,

    /// <summary>
    /// Uses the GLFW (Graphics Library Framework) backend.
    /// </summary>
    Glfw = 2,

    /// <summary>
    /// Uses the SDL (Simple DirectMedia Layer) backend.
    /// </summary>
    Sdl = 3
}

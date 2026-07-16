using Hypercube.Utilities;

namespace Hypercube.Windowing.Core;

/// <summary>
/// Provides global constants for the windowing system.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Gets the array of windowing backends that are currently implemented and available for use.
    /// </summary>
    public static readonly WindowingBackendType[] ImplementedBackends =
    [
        WindowingBackendType.Glfw
    ];
    
    /// <summary>
    /// Gets a mapping of operating systems to their respective optimal and supported windowing backends.
    /// </summary>
    public static readonly Dictionary<OS, OSBackendInfo> OSBackendMatrix = new()
    {
        {
            // If we're running on some piece of crap
            // we assume it supports Glfw.
            // Auto is still the safest universal baseline.
            OS.Unknown, new OSBackendInfo(
                Best: WindowingBackendType.Glfw,
                Supported: [
                    WindowingBackendType.Glfw
                ]
            )
        },
        {
            OS.Windows, new OSBackendInfo(
                Best: WindowingBackendType.Sdl,
                Supported:
                [
                    WindowingBackendType.Sdl,
                    WindowingBackendType.Glfw
                ]
            )
        },
        {
            OS.Linux, new OSBackendInfo(
                Best: WindowingBackendType.Sdl,
                Supported:
                [
                    WindowingBackendType.Sdl,
                    WindowingBackendType.Glfw
                ]
            )
        }
    };
}

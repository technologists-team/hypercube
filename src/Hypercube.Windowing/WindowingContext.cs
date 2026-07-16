using System.Runtime.CompilerServices;
using Hypercube.Utilities;
using Hypercube.Windowing.Core;
using Hypercube.Windowing.Core.Device;
using Hypercube.Windowing.Device;

namespace Hypercube.Windowing;

/// <summary>
/// Provides a static entry point for creating and managing windowing devices.
/// </summary>
public static class WindowingContext
{
    /// <summary>
    /// Creates a new windowing device instance with the specified settings.
    /// </summary>
    /// <param name="settings">The configuration settings for the windowing device.</param>
    /// <returns>A new instance of <see cref="IWindowingDevice"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IWindowingDevice Create(in WindowingDeviceSettings settings)
    {
        var type = ResolveBackend(settings.Backend, settings.BackendForced);
        return new WindowingDevice(settings with { Backend = type });
    }
    
    private static WindowingBackendType ResolveBackend(WindowingBackendType backendType = WindowingBackendType.Auto, bool forced = false)
    {
        var os = HyperOS.Current;
        
        // Something terrible going on
        if (!Constants.OSBackendMatrix.TryGetValue(os, out var info))
            return WindowingBackendType.None;
        
        if (backendType != WindowingBackendType.Auto)
        {
            if (info.Supported.Contains(backendType))
            {
                if (TryChoose(backendType))
                    return backendType;
            }
            
            if (forced)
                return WindowingBackendType.None;
        }
        
        if (TryChoose(info.Best))
            return info.Best;
            
        foreach (var supportedBackend in info.Supported)
        {
            if (TryChoose(supportedBackend))
                return supportedBackend;
        }

        return WindowingBackendType.None;
        
        bool TryChoose(WindowingBackendType back) => Constants.ImplementedBackends.Contains(back);
    }
}

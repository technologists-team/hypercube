using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Device;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Types;
using Hypercube.Utilities;

namespace Hypercube.Graphics;

public static class GraphicsContext
{
    public static IGraphicDevice Create(in GraphicsDeviceSettings settings)
    {
        var type = ResolveBackend(settings.Backend, settings.BackendForced);
        return new GraphicDevice(settings with { Backend = type });
    }
    
    private static RenderBackendType ResolveBackend(RenderBackendType backendType = RenderBackendType.Auto, bool forced = false)
    {
        var os = HyperOS.Current;
        
        // Something terrible going on
        if (!Constants.OSBackendMatrix.TryGetValue(os, out var info))
            return RenderBackendType.None;
        
        if (backendType != RenderBackendType.Auto)
        {
            if (info.Supported.Contains(backendType))
            {
                if (TryChoose(backendType))
                    return backendType;
            }
            
            if (forced)
                return RenderBackendType.None;
        }
        
        if (TryChoose(info.Best))
            return info.Best;
            
        foreach (var supportedBackend in info.Supported)
        {
            if (TryChoose(supportedBackend))
                return supportedBackend;
        }

        return RenderBackendType.None;
        
        bool TryChoose(RenderBackendType back) => Constants.ImplementedBackends.Contains(back);
    }
}

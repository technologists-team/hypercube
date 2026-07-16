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
    
    private static BackendType ResolveBackend(BackendType? type, bool forced = false)
    {
        var os = HyperOS.Current;
        
        // Something terrible going on
        if (!Constants.OSBackendMatrix.TryGetValue(os, out var info))
            return BackendType.None;
        
        if (type is not null)
        {
            if (info.Supported.Contains(type.Value))
            {
                if (TryChoose(type.Value))
                    return type.Value;
            }
            
            if (forced)
                return BackendType.None;
        }
        
        if (TryChoose(info.Best))
            return info.Best;
            
        foreach (var supportedBackend in info.Supported)
        {
            if (TryChoose(supportedBackend))
                return supportedBackend;
        }

        return BackendType.None;
        
        static bool TryChoose(BackendType back) => Constants.ImplementedBackends.Contains(back);
    }
}

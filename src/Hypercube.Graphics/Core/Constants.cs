using Hypercube.Graphics.Core.Backends;
using Hypercube.Utilities;

namespace Hypercube.Graphics.Core;

public static class Constants
{
    public static readonly (string, uint)[] ShaderAttribLocations =
    [
        ("aPos",       0),
        ("aColor",     1),
        ("aTexCoords", 2),
        ("aNormal",    3)
    ];

    public static readonly RenderBackend[] ImplementedBackends =
    [
        RenderBackend.OpenGL
    ];
    
    public static readonly Dictionary<OS, OSBackendInfo> OSBackendMatrix = new()
    {
        {
            // If we're running on some piece of crap
            // we assume it supports OpenGL.
            // OpenGL is still the safest universal baseline
            OS.Unknown, new OSBackendInfo(
                Best: RenderBackend.OpenGL,
                Supported: [RenderBackend.OpenGL]
            )
        },
        {
            OS.Windows, new OSBackendInfo(
                Best: RenderBackend.Direct3D12,
                Supported:
                [
                    RenderBackend.Direct3D12,
                    RenderBackend.Direct3D11,
                    RenderBackend.Vulkan,
                    RenderBackend.OpenGL
                ]
            )
        },
        {
            OS.Linux, new OSBackendInfo(
                Best: RenderBackend.Vulkan,
                Supported:
                [
                    RenderBackend.Vulkan,
                    RenderBackend.OpenGL,
                    RenderBackend.OpenGles
                ]
            )
        },
        {
            OS.MacOS, new OSBackendInfo(
                Best: RenderBackend.Metal,
                Supported:
                [
                    RenderBackend.Metal,
                    RenderBackend.Vulkan, // via MoltenVK
                    RenderBackend.OpenGL
                ]
            )
        },
        {
            OS.Android, new OSBackendInfo(
                Best: RenderBackend.Vulkan,
                Supported:
                [
                    RenderBackend.Vulkan,
                    RenderBackend.OpenGles
                ]
            )
        },
        {
            OS.Browser, new OSBackendInfo(
                Best: RenderBackend.WebGpu,
                Supported:
                [
                    RenderBackend.WebGpu,
                    RenderBackend.OpenGles
                ]
            )
        }
    };
}

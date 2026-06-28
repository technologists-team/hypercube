using Hypercube.Graphics.Core.Types;
using Hypercube.Graphics.Types;
using Hypercube.Utilities;

namespace Hypercube.Graphics.Core;

public static class Constants
{
    public static readonly VertexAttribute[] ShaderAttribLocations =
    [
        new("aPos",       0, 3, VertexAttributeType.Float),
        new("aColor",     1, 4, VertexAttributeType.Float),
        new("aTexCoords", 2, 2, VertexAttributeType.Float),
        new("aNormal",    3, 3, VertexAttributeType.Float)
    ];

    public static readonly RenderBackendType[] ImplementedBackends =
    [
        RenderBackendType.OpenGL
    ];
    
    public static readonly Dictionary<OS, OSBackendInfo> OSBackendMatrix = new()
    {
        {
            // If we're running on some piece of crap
            // we assume it supports OpenGL.
            // OpenGL is still the safest universal baseline
            OS.Unknown, new OSBackendInfo(
                Best: RenderBackendType.OpenGL,
                Supported: [RenderBackendType.OpenGL]
            )
        },
        {
            OS.Windows, new OSBackendInfo(
                Best: RenderBackendType.Direct3D12,
                Supported:
                [
                    RenderBackendType.Direct3D12,
                    RenderBackendType.Direct3D11,
                    RenderBackendType.Vulkan,
                    RenderBackendType.OpenGL
                ]
            )
        },
        {
            OS.Linux, new OSBackendInfo(
                Best: RenderBackendType.Vulkan,
                Supported:
                [
                    RenderBackendType.Vulkan,
                    RenderBackendType.OpenGL,
                    RenderBackendType.OpenGles
                ]
            )
        },
        {
            OS.MacOS, new OSBackendInfo(
                Best: RenderBackendType.Metal,
                Supported:
                [
                    RenderBackendType.Metal,
                    RenderBackendType.Vulkan, // via MoltenVK
                    RenderBackendType.OpenGL
                ]
            )
        },
        {
            OS.Android, new OSBackendInfo(
                Best: RenderBackendType.Vulkan,
                Supported:
                [
                    RenderBackendType.Vulkan,
                    RenderBackendType.OpenGles
                ]
            )
        },
        {
            OS.Browser, new OSBackendInfo(
                Best: RenderBackendType.WebGpu,
                Supported:
                [
                    RenderBackendType.WebGpu,
                    RenderBackendType.OpenGles
                ]
            )
        }
    };
}

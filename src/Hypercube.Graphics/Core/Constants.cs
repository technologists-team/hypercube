using Hypercube.Graphics.Core.Types;
using Hypercube.Graphics.Types;
using Hypercube.Utilities;

namespace Hypercube.Graphics.Core;

public static class Constants
{
    public const int BufferVerticesSize = 65536;
    public const int BufferIndicesSize = 98304;
        
    public const int MinVertexCapacity = 1024;
    public const int MinIndexCapacity = 2048;

    public static readonly ProjectMetadata Engine = new()
    {
        Name = "Hypercube",
        Version = new Version(3, 0, 0),
    };

    public static readonly ProjectMetadata Application = new()
    {
        Name = "Hypercube App",
        Version = new Version(1, 0, 0),
    };
    
    public static readonly VertexAttribute[] ShaderAttribLocations =
    [
        new("aPos",       0, 3, VertexAttributeType.Float),
        new("aColor",     1, 4, VertexAttributeType.Float),
        new("aTexCoords", 2, 2, VertexAttributeType.Float),
        new("aNormal",    3, 3, VertexAttributeType.Float)
    ];

    public static readonly BackendType[] ImplementedBackends =
    [
        BackendType.OpenGl,
        BackendType.OpenGles,
        BackendType.Vulkan
    ];
    
    public static readonly Dictionary<OS, OSBackendInfo> OSBackendMatrix = new()
    {
        {
            // If we're running on some piece of crap
            // we assume it supports OpenGL.
            // OpenGL is still the safest universal baseline
            OS.Unknown, new OSBackendInfo(
                Best: BackendType.OpenGl,
                Supported: [BackendType.OpenGl]
            )
        },
        {
            OS.Windows, new OSBackendInfo(
                Best: BackendType.Direct3D12,
                Supported:
                [
                    BackendType.Direct3D12,
                    BackendType.Direct3D11,
                    BackendType.Vulkan,
                    BackendType.OpenGl
                ]
            )
        },
        {
            OS.Linux, new OSBackendInfo(
                Best: BackendType.Vulkan,
                Supported:
                [
                    BackendType.Vulkan,
                    BackendType.OpenGl,
                    BackendType.OpenGles
                ]
            )
        },
        {
            OS.MacOS, new OSBackendInfo(
                Best: BackendType.Metal,
                Supported:
                [
                    BackendType.Metal,
                    BackendType.Vulkan, // via MoltenVK
                    BackendType.OpenGl
                ]
            )
        },
        {
            OS.Android, new OSBackendInfo(
                Best: BackendType.Vulkan,
                Supported:
                [
                    BackendType.Vulkan,
                    BackendType.OpenGles
                ]
            )
        },
        {
            OS.Browser, new OSBackendInfo(
                Best: BackendType.WebGpu,
                Supported:
                [
                    BackendType.WebGpu,
                    BackendType.OpenGles
                ]
            )
        }
    };
}

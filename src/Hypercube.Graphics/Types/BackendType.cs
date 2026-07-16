namespace Hypercube.Graphics.Types;

public enum BackendType : byte
{
    None        = 0,

    // Desktop / Cross-platform
    OpenGl      = 2,
    Vulkan      = 3,
    Direct3D11  = 4,
    Direct3D12  = 5,
    Metal       = 6,

    // Web / Embedded
    OpenGles    = 7,
    WebGpu      = 8,
}

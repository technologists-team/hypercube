namespace Hypercube.Graphics.Core.Types;

[Flags]
public enum ClearBufferMask : byte
{
    None             = 1 << 0,
    DepthBuffer      = 1 << 1,
    StencilBuffer    = 1 << 2,
    ColorBuffer      = 1 << 3,
    CoverageBufferNv = 1 << 4,
}

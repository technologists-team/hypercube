using Hypercube.Graphics.Types;

namespace Hypercube.Graphics.Core.Types;

public readonly record struct OSBackendInfo(
    RenderBackendType Best, 
    RenderBackendType[] Supported
);

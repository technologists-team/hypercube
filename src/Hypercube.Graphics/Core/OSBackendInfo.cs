using Hypercube.Graphics.Core.Backends;

namespace Hypercube.Graphics.Core;

public readonly record struct OSBackendInfo(
    RenderBackend Best, 
    RenderBackend[] Supported
);

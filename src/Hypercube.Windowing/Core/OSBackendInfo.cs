namespace Hypercube.Windowing.Core;

public readonly record struct OSBackendInfo(
    WindowingBackendType Best, 
    WindowingBackendType[] Supported
);

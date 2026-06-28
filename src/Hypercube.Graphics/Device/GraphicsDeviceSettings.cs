using Hypercube.Graphics.Types;

namespace Hypercube.Graphics.Device;

public struct GraphicsDeviceSettings
{
    public RenderBackendType Backend { get; init; }
    public bool BackendForced { get; init; }
    
    public GetProcAddress GetProcAddress { get; init; }
}

public delegate nint GetProcAddress(string name);

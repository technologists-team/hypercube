using Hypercube.Graphics.Core;
using Hypercube.Graphics.Types;

namespace Hypercube.Graphics.Device;

public struct GraphicsDeviceSettings()
{
    public ProjectMetadata Application { get; init; } = Constants.Application;
    public ProjectMetadata Engine { get; init; } = Constants.Engine;
    
    public BackendType? Backend { get; init; }
    public bool BackendForced { get; init; }
    
    public GetProcAddress GetProcAddress { get; init; }
    public nint? Context { get; init; }
}

public delegate nint GetProcAddress(string name);

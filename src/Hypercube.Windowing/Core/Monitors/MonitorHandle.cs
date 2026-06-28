using Hypercube.Utilities.Attributes;

namespace Hypercube.Windowing.Core.Monitors;

[IdStruct(typeof(nint))]
public readonly partial struct MonitorHandle
{
    public static readonly MonitorHandle Null = new(nint.Zero);
}

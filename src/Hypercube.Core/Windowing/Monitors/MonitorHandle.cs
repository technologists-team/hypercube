using Hypercube.Utilities.Attributes;

namespace Hypercube.Core.Windowing.Monitors;

[IdStruct(typeof(nint))]
public partial struct MonitorHandle
{
    public static readonly MonitorHandle Zero = new(nint.Zero);
}

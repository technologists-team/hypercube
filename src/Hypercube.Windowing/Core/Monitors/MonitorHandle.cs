using Hypercube.Utilities.Attributes;

namespace Hypercube.Windowing.Core.Monitors;

/// <summary>
/// Represents a strongly-typed handle for a physical monitor, backed by a native pointer.
/// </summary>
[IdStruct(typeof(nint))]
public readonly partial struct MonitorHandle
{
    /// <summary>
    /// Represents an invalid or uninitialized monitor handle.
    /// </summary>
    public static readonly MonitorHandle Null = new(nint.Zero);
}

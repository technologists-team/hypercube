using Hypercube.Utilities.Attributes;

namespace Hypercube.Windowing.Core.Windows;

/// <summary>
/// Represents a strongly-typed handle for a window, backed by a native pointer.
/// </summary>
[IdStruct(typeof(nint))]
public readonly partial struct WindowHandle
{
    /// <summary>
    /// Represents an invalid or uninitialized window handle.
    /// </summary>
    public static readonly WindowHandle Null = new(nint.Zero);
}

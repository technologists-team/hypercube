using Hypercube.Utilities.Attributes;

namespace Hypercube.Core.Windowing.Windows;

/// <summary>
/// Represents a strongly-typed handle to a native window.
/// Wraps a platform-specific pointer or identifier.
/// </summary>
[IdStruct(typeof(nint))]
public readonly partial struct WindowHandle
{
    /// <summary>
    /// A null/invalid window handle.
    /// </summary>
    public static readonly WindowHandle Zero = new(nint.Zero);
}

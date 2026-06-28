using Hypercube.Utilities.Attributes;

namespace Hypercube.Windowing.Core.Windows;

[IdStruct(typeof(nint))]
public readonly partial struct WindowHandle
{
    public static readonly WindowHandle Null = new(nint.Zero);
}

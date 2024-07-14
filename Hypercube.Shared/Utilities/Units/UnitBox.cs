using System.Runtime.InteropServices;

namespace Hypercube.Shared.Utilities.Units;

/// <summary>
/// Whenever you see this it is some other object, it should be resolved using Unsafe.As()
/// </summary>
/// <remarks>
/// Should be used whenever we want to pass value by ref
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public sealed class UnitBox
{
    public Unit Value;
}
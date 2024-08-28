using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Units;

/// <summary>
/// Whenever you see this it is some other object, it should be resolved using Unsafe.As().
/// </summary>
/// <remarks>
/// Should be used whenever we want to pass value by ref.
/// </remarks>
[PublicAPI, StructLayout(LayoutKind.Sequential)]
public sealed class UnitBox
{
    public Unit Value;
    
    public static implicit operator Unit(UnitBox unitBox)
    {
        return unitBox.Value;
    }
}
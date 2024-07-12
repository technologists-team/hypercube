using System.Runtime.CompilerServices;
using Hypercube.Shared.Utilities.Units;

namespace Hypercube.Shared.Utilities;

public static class UnitHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Unit ExtractUnitRef<T>(ref T eventArgs, Type objType)
    {
        return ref Unsafe.As<T, Unit>(ref eventArgs);
    }
}
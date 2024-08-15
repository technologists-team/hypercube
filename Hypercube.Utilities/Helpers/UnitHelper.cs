using System.Runtime.CompilerServices;
using Hypercube.Utilities.Units;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public static class UnitHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Unit ExtractUnitRef<T>(ref T eventArgs, Type objType)
    {
        return ref Unsafe.As<T, Unit>(ref eventArgs);
    }
}
using System.Runtime.InteropServices;

namespace Hypercube.Shared.Utilities.Helpers;

public sealed class MarshalHelper
{
    public static int SizeOf<T>() where T : struct
    {
        return Marshal.SizeOf(default(T));
    }
}
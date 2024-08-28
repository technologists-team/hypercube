using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public sealed class MarshalHelper
{
    public static int SizeOf<T>() where T : struct
    {
        return Marshal.SizeOf(default(T));
    }
}
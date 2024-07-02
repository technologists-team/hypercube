using System.Runtime.CompilerServices;

namespace Hypercube.Math.Vector;

public readonly partial struct Vector2
{
    /// <summary>
    /// 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(System.Numerics.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
}
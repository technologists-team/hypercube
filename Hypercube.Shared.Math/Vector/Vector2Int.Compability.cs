using System.Runtime.CompilerServices;

namespace Hypercube.Math.Vector;

public readonly partial struct Vector2Int
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2Int(System.Drawing.Size size)
    {
        return new Vector2Int(size.Width, size.Height);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Drawing.Size(Vector2Int vector2)
    {
        return new System.Drawing.Size(vector2.X, vector2.Y);
    }
} 
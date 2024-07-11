using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Vector;

public readonly partial struct Vector2Int
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector2Int vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector2Int vector)
    {
        return new Vector3(vector.X, vector.Y, 0f);
    }
    
    /*
     * Tuple Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2Int((int x, int y) a)
    {
        return new Vector2Int(a.x, a.y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (int x, int y)(Vector2Int a)
    {
        return (a.X, a.Y);
    }
    
    /*
     * System.Drawing Compatibility
     */
    
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
    
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2Int(OpenTK.Mathematics.Vector2i vector)
    {
        return new Vector2Int(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Vector2i(Vector2Int vector)
    {
        return new OpenTK.Mathematics.Vector2i(vector.X, vector.Y);
    }
    
    /*
     * OpenToolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2Int(OpenToolkit.Mathematics.Vector2i vector)
    {
        return new Vector2Int(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Vector2i(Vector2Int vector)
    {
        return new OpenToolkit.Mathematics.Vector2i(vector.X, vector.Y);
    }
} 
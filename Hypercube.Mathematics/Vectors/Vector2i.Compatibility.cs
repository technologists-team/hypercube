using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Vectors;

public readonly partial struct Vector2i
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector2i vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector2i vector)
    {
        return new Vector3(vector.X, vector.Y, 0f);
    }
    
    /*
     * Tuple Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i((int x, int y) a)
    {
        return new Vector2i(a.x, a.y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (int x, int y)(Vector2i a)
    {
        return (a.X, a.Y);
    }
    
    /*
     * System.Numerics Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i(System.Numerics.Vector2 vector)
    {
        return new Vector2i((int)vector.X, (int)vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector2(Vector2i vector)
    {
        return new System.Numerics.Vector2(vector.X, vector.Y);
    }
    
    /*
     * System.Drawing Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i(System.Drawing.Size size)
    {
        return new Vector2i(size.Width, size.Height);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Drawing.Size(Vector2i vector2)
    {
        return new System.Drawing.Size(vector2.X, vector2.Y);
    }
    
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i(OpenTK.Mathematics.Vector2i vector)
    {
        return new Vector2i(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Vector2i(Vector2i vector)
    {
        return new OpenTK.Mathematics.Vector2i(vector.X, vector.Y);
    }
    
    /*
     * OpenToolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i(OpenToolkit.Mathematics.Vector2i vector)
    {
        return new Vector2i(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Vector2i(Vector2i vector)
    {
        return new OpenToolkit.Mathematics.Vector2i(vector.X, vector.Y);
    }
} 
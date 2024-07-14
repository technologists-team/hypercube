using System.Runtime.CompilerServices;

namespace Hypercube.Math.Vectors;

public readonly partial struct Vector2
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2Int(Vector2 vector)
    {
        return new Vector2Int((int)vector.X, (int)vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector2 vector)
    {
        return new Vector3(vector.X, vector.Y, 0f);
    }
    
    /*
     * Tuple Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2((float x, float y) a)
    {
        return new Vector2(a.x, a.y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float x, float y)(Vector2 a)
    {
        return (a.X, a.Y);
    }
    
    /*
     * System.Numerics Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(System.Numerics.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector2(Vector2 vector)
    {
        return new System.Numerics.Vector2(vector.X, vector.Y);
    }
    
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(OpenTK.Mathematics.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Vector2(Vector2 vector)
    {
        return new OpenTK.Mathematics.Vector2(vector.X, vector.Y);
    }
    
    /*
     * OpenToolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(OpenToolkit.Mathematics.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Vector2(Vector2 vector)
    {
        return new OpenToolkit.Mathematics.Vector2(vector.X, vector.Y);
    }
}
using System.Runtime.CompilerServices;

namespace Hypercube.Math.Matrices;

public partial struct Matrix3X2
{
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Matrix3x2(Matrix3X2 matrix3)
    {
        return new OpenTK.Mathematics.Matrix3x2(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3X2(OpenTK.Mathematics.Matrix3x2 matrix3)
    {
        return new Matrix3X2(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    /*
     * OpenToolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Matrix3x2(Matrix3X2 matrix3)
    {
        return new OpenToolkit.Mathematics.Matrix3x2(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3X2(OpenToolkit.Mathematics.Matrix3x2 matrix3)
    {
        return new Matrix3X2(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
}
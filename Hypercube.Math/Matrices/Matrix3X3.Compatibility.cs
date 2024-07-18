using System.Runtime.CompilerServices;

namespace Hypercube.Math.Matrices;

public partial struct Matrix3X3
{
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Matrix3(Matrix3X3 matrix3)
    {
        return new OpenTK.Mathematics.Matrix3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3X3(OpenTK.Mathematics.Matrix3 matrix3)
    {
        return new Matrix3X3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    /*
     * OpenToolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Matrix3(Matrix3X3 matrix3)
    {
        return new OpenToolkit.Mathematics.Matrix3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3X3(OpenToolkit.Mathematics.Matrix3 matrix3)
    {
        return new Matrix3X3(matrix3.Row0, matrix3.Row1, matrix3.Row2);
    }
}
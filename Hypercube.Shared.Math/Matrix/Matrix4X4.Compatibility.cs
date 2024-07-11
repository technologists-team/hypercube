using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace Hypercube.Shared.Math.Matrix;

public partial struct Matrix4X4
{
    /*
     * OpenTK Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Matrix4(Matrix4X4 matrix4X4)
    {
        return new OpenTK.Mathematics.Matrix4(matrix4X4.Row0, matrix4X4.Row1, matrix4X4.Row2, matrix4X4.Row3);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix4X4(Matrix4 matrix4)
    {
        return new Matrix4X4(matrix4.Row0, matrix4.Row1, matrix4.Row2, matrix4.Row3);
    }

    /*
     * Open Toolkit Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenToolkit.Mathematics.Matrix4(Matrix4X4 matrix4X4)
    {
        return new OpenToolkit.Mathematics.Matrix4(matrix4X4.Row0, matrix4X4.Row1, matrix4X4.Row2, matrix4X4.Row3);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix4X4(OpenToolkit.Mathematics.Matrix4 matrix4)
    {
        return new Matrix4X4(matrix4.Row0, matrix4.Row1, matrix4.Row2, matrix4.Row3);
    }
}
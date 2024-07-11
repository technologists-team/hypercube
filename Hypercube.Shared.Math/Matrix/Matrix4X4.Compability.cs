using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Matrix;

public partial struct Matrix4X4
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Matrix4(Matrix4X4 matrix4X4)
    {
        return new OpenTK.Mathematics.Matrix4(matrix4X4.Row0, matrix4X4.Row1, matrix4X4.Row2, matrix4X4.Row3);
    }
}
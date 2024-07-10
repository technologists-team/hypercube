using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Matrix;

public partial struct Matrix4X4
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator OpenTK.Mathematics.Matrix4(Matrix4X4 matrix4X4)
    {
        return new OpenTK.Mathematics.Matrix4(matrix4X4.Raw0, matrix4X4.Raw1, matrix4X4.Raw2, matrix4X4.Raw3);
    }
}
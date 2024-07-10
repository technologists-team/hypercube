using System.Runtime.CompilerServices;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math.Matrix;

public struct Matrix4X4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
{
    public static Matrix4X4 Zero => new(Vector4.Zero);
    public static Matrix4X4 One => new(Vector4.One);
    public static Matrix4X4 Identity => new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1);
    
    public Vector4 Raw0 = x;
    public Vector4 Raw1 = y;
    public Vector4 Raw2 = z;
    public Vector4 Raw3 = w;

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public float M00
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0.WithX(value);
    }
    
    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public float M01
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public float M02
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public float M03
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0.WithW(value);
    }
    
    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public float M10
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1.WithX(value);
    }
    
    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public float M11
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1.WithY(value);
    }
    
    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public float M12
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1.WithZ(value);
    }
    
    /// <summary>
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public float M13
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1.WithW(value);
    }
    
    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public float M20
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2.WithX(value);
    }
    
    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public float M21
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2.WithY(value);
    }
    
    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public float M22
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2.WithZ(value);
    }
    
    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public float M23
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2.WithW(value);
    }
    
    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public float M30
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3.WithX(value);
    }
    
    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public float M31
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3.WithY(value);
    }
    
    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public float M32 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3.WithZ(value);
    }
    
    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public float M33
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3.WithW(value);
    }
    
    public Matrix4X4(Vector4 value) : this(value, value, value, value)
    {
    }
    
    public Matrix4X4(float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33) : this(new Vector4(m00, m01, m02, m03),
        new Vector4(m10, m11, m12, m13),
        new Vector4(m20, m21, m22, m23),
        new Vector4(m30, m31, m32, m33))
    {
    }

    /// <summary>
    /// Creating perspective fov matrix
    /// <code>
    /// hFov = fov / 2;
    /// hFovX = hFov * aspect;
    ///
    /// 
    /// zDelta = zFar - zNear;
    /// z1 = zFar / zDelta;
    /// z2 = zFar * zNear / zDelta;
    /// 
    ///  cot(hFovX) |     0     |   0  |   0
    ///       0     | cot(hFov) |   0  |   0
    ///       0     |     0     |  z1  |   1
    ///       0     |     0     |  z2  |   0
    /// </code>
    /// </summary>
    public static Matrix4X4 CreatePerspective(float fov, float aspect, float zNear, float zFar)
    {
        var result = Zero;

        var halfFov = fov / 2;
        var tanHalfFov = (float)System.Math.Tan(halfFov);
        var zDelta = zFar - zNear;
        
        result.M00 = 1 / (aspect * tanHalfFov);
        result.M11 = 1 / tanHalfFov;
        result.M22 = zFar / zDelta;
        result.M23 = 1;
        result.M32 = zFar * zNear / zDelta;
        
        return result;
    }
    
    public override string ToString()
    {
        return $"{Raw0}\n{Raw1}\n{Raw2}\n{Raw3}";
    }
}
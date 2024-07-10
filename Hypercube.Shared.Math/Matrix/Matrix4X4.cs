using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math.Matrix;

[StructLayout(LayoutKind.Sequential)]
public partial struct Matrix4X4(Vector4 x, Vector4 y, Vector4 z, Vector4 w) : IEquatable<Matrix4X4>
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

    public Vector4 Column0 => new(M00, M10, M20, M30);
    public Vector4 Column1 => new(M01, M11, M21, M31);
    public Vector4 Column2 => new(M02, M12, M22, M32);
    public Vector4 Column3 => new(M03, M13, M23, M33);

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public float M00
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0 = Raw0.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public float M01
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0 = Raw0.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public float M02
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0 = Raw0.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public float M03
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw0.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw0 = Raw0.WithW(value);
    }

    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public float M10
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1 = Raw1.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public float M11
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1 = Raw1.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public float M12
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1 = Raw1.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public float M13
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw1.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw1 = Raw1.WithW(value);
    }

    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public float M20
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2 = Raw2.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public float M21
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2 = Raw2.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public float M22
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2 = Raw2.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public float M23
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw2.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw2 = Raw2.WithW(value);
    }

    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public float M30
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3 = Raw3.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public float M31
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3 = Raw3.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public float M32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3 = Raw3.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public float M33
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw3.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Raw3 = Raw3.WithW(value);
    }

    public Matrix4X4(Vector4 value) : this(value, value, value, value)
    {
    }

    /// <summary>
    /// Creating matrix
    /// <code>
    ///  M00  |  M01  |  M02  |  M03 
    ///  M10  |  M11  |  M12  |  M13
    ///  M20  |  M21  |  M22  |  M23
    ///  M30  |  M31  |  M32  |  M33
    /// </code>
    /// </summary>
    public Matrix4X4(float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33) : this(new Vector4(m00, m01, m02, m03),
        new Vector4(m10, m11, m12, m13),
        new Vector4(m20, m21, m22, m23),
        new Vector4(m30, m31, m32, m33))
    {
    }

    public Matrix4X4(Matrix4X4 matrix4X4) : this(matrix4X4.Raw0, matrix4X4.Raw1, matrix4X4.Raw2, matrix4X4.Raw3)
    {
    }

    public bool Equals(Matrix4X4 other)
    {
        return Raw0.Equals(other.Raw0) &&
               Raw1.Equals(other.Raw1) &&
               Raw2.Equals(other.Raw2) &&
               Raw3.Equals(other.Raw3);
    }

    public override bool Equals(object? obj)
    {
        return obj is Matrix4X4 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Raw0, Raw1, Raw2, Raw3);
    }

    public override string ToString()
    {
        return $"{Raw0}\n{Raw1}\n{Raw2}\n{Raw3}";
    }

    public static Matrix4X4 operator *(Matrix4X4 a, Matrix4X4 b)
    {
        var result = Zero;

        result.M00 = (a.Raw0 * b.Column0).Sum();
        result.M01 = (a.Raw0 * b.Column1).Sum();
        result.M02 = (a.Raw0 * b.Column2).Sum();
        result.M03 = (a.Raw0 * b.Column3).Sum();

        result.M10 = (a.Raw1 * b.Column0).Sum();
        result.M11 = (a.Raw1 * b.Column1).Sum();
        result.M12 = (a.Raw1 * b.Column2).Sum();
        result.M13 = (a.Raw1 * b.Column3).Sum();

        result.M20 = (a.Raw2 * b.Column0).Sum();
        result.M21 = (a.Raw2 * b.Column1).Sum();
        result.M22 = (a.Raw2 * b.Column2).Sum();
        result.M23 = (a.Raw2 * b.Column3).Sum();

        result.M30 = (a.Raw3 * b.Column0).Sum();
        result.M31 = (a.Raw3 * b.Column1).Sum();
        result.M32 = (a.Raw3 * b.Column2).Sum();
        result.M33 = (a.Raw3 * b.Column3).Sum();

        return result;
    }

    public static Vector4 operator *(Matrix4X4 a, Vector4 b)
    {
        return new Vector4(
            (a.Raw0 * b).Sum(),
            (a.Raw1 * b).Sum(),
            (a.Raw2 * b).Sum(),
            (a.Raw3 * b).Sum());
    }

    public static bool operator ==(Matrix4X4 a, Matrix4X4 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Matrix4X4 a, Matrix4X4 b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4X4 Transpose(Matrix4X4 matrix4X4)
    {
        return new Matrix4X4(matrix4X4.Column0, matrix4X4.Column1, matrix4X4.Column2, matrix4X4.Column3);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  v  |  0  |  0  |  0 
    ///  0  |  v  |  0  |  0
    ///  0  |  0  |  v  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateScale(float value)
    {
        return CreateScale(value, value, value);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0  |  0 
    ///  0  |  y  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateScale(Vector2 scale)
    {
        return CreateScale(scale.X, scale.Y, 1f);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0  |  0 
    ///  0  |  y  |  0  |  0
    ///  0  |  0  |  z  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateScale(Vector3 scale)
    {
        return CreateScale(scale.X, scale.Y, scale.Z);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0  |  0 
    ///  0  |  y  |  0  |  0
    ///  0  |  0  |  z  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateScale(float x, float y, float z)
    {
        var result = Identity;

        result.M00 = x;
        result.M11 = y;
        result.M22 = z;

        return result;
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  x 
    ///  0  |  1  |  0  |  y
    ///  0  |  0  |  1  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateTranslate(Vector2 vector2)
    {
        return CreateTranslate(vector2.X, vector2.Y, 0f);
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  x 
    ///  0  |  1  |  0  |  y
    ///  0  |  0  |  1  |  z
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateTranslate(Vector3 vector3)
    {
        return CreateTranslate(vector3.X, vector3.Y, vector3.Z);
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  x 
    ///  0  |  1  |  0  |  y
    ///  0  |  0  |  1  |  z
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateTranslate(float x, float y, float z)
    {
        var result = Identity;

        result.M03 = x;
        result.M13 = y;
        result.M23 = z;

        return result;
    }
    /// <summary>
    /// Creating orthographic matrix
    /// <code>
    ///  2 / (r - l) |      0      |       0        |    -(r + l) / (r - l)
    ///       0      | 2 / (t - b) |       0        |    -(t + b) / (t - b)
    ///       0      |      0      | -2 / (zF - zN) |  -(zF + zN) / (zF - zN)
    ///       0      |      0      |       0        |             1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateOrthographic(Box2 box2, float zNear, float zFar)
    {
        var result = Identity;

        result.M00 = 2 / (box2.Right - box2.Left);
        result.M11 = 2 / (box2.Top - box2.Bottom);
        result.M22 = -2 / (zFar - zNear);

        result.M03 = -(box2.Right + box2.Left) / (box2.Right - box2.Left);
        result.M13 = -(box2.Top + box2.Bottom) / (box2.Top - box2.Bottom);
        result.M23 = -(zFar + zNear) / (zFar - zNear);

        return result;
    }

    /// <summary>
    /// Creating perspective matrix
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
}

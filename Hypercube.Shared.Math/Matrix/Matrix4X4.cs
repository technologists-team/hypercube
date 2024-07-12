using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Transform;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math.Matrix;

[StructLayout(LayoutKind.Sequential)]
public partial struct Matrix4X4 : IEquatable<Matrix4X4>
{
    public static Matrix4X4 Zero => new(Vector4.Zero);
    public static Matrix4X4 One => new(Vector4.One);
    public static Matrix4X4 Identity => new(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

    public Vector4 Row0;
    public Vector4 Row1;
    public Vector4 Row2;
    public Vector4 Row3;

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
        get => Row0.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row0 = Row0.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public float M01
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row0 = Row0.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public float M02
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row0 = Row0.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public float M03
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row0 = Row0.WithW(value);
    }

    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public float M10
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row1 = Row1.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public float M11
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row1 = Row1.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public float M12
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row1 = Row1.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public float M13
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row1 = Row1.WithW(value);
    }

    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public float M20
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row2 = Row2.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public float M21
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row2 = Row2.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public float M22
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row2 = Row2.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public float M23
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row2 = Row2.WithW(value);
    }

    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public float M30
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row3 = Row3.WithX(value);
    }

    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public float M31
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row3 = Row3.WithY(value);
    }

    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public float M32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row3 = Row3.WithZ(value);
    }

    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public float M33
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Row3 = Row3.WithW(value);
    }
    /// <summary>
    /// Creates new matrix 4x4
    /// <code>
    /// Row0.X  |  Row0.Y  |  Row0.Z  |  Row0.W 
    /// Row1.X  |  Row1.Y  |  Row1.Z  |  Row1.W 
    /// Row2.X  |  Row2.Y  |  Row2.Z  |  Row2.W 
    /// Row2.X  |  Row2.Y  |  Row2.Z  |  Row2.W
    /// </code>
    /// </summary>
    /// <param name="x">Row 0</param>
    /// <param name="y">Row 1</param>
    /// <param name="z">Row 2</param>
    /// <param name="w">Row 3</param>
    public Matrix4X4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
    {
        Row0 = x;
        Row1 = y;
        Row2 = z;
        Row3 = w;
    }
    /// <summary>
    /// Creates new matrix with all rows is "<paramref name="value"/>"
    /// </summary>
    /// <param name="value">Vector4 to make rows out of</param>
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

    public Matrix4X4(Matrix4X4 matrix4X4) : this(matrix4X4.Row0, matrix4X4.Row1, matrix4X4.Row2, matrix4X4.Row3)
    {
    }

    public bool Equals(Matrix4X4 other)
    {
        return Row0.Equals(other.Row0) &&
               Row1.Equals(other.Row1) &&
               Row2.Equals(other.Row2) &&
               Row3.Equals(other.Row3);
    }

    public float[] ToArray()
    {
        return new float[]
        {
            M00, M01, M02, M03,
            M10, M11, M12, M13,
            M20, M21, M22, M23,
            M30, M31, M32, M33
        };
    }

    public override bool Equals(object? obj)
    {
        return obj is Matrix4X4 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row0, Row1, Row2, Row3);
    }

    public override string ToString()
    {
        return $"{Row0}\n{Row1}\n{Row2}\n{Row3}";
    }

    public static Matrix4X4 operator *(Matrix4X4 a, Matrix4X4 b)
    {
        var result = Zero;

        result.M00 = (a.Row0 * b.Column0).Sum();
        result.M01 = (a.Row0 * b.Column1).Sum();
        result.M02 = (a.Row0 * b.Column2).Sum();
        result.M03 = (a.Row0 * b.Column3).Sum();

        result.M10 = (a.Row1 * b.Column0).Sum();
        result.M11 = (a.Row1 * b.Column1).Sum();
        result.M12 = (a.Row1 * b.Column2).Sum();
        result.M13 = (a.Row1 * b.Column3).Sum();

        result.M20 = (a.Row2 * b.Column0).Sum();
        result.M21 = (a.Row2 * b.Column1).Sum();
        result.M22 = (a.Row2 * b.Column2).Sum();
        result.M23 = (a.Row2 * b.Column3).Sum();

        result.M30 = (a.Row3 * b.Column0).Sum();
        result.M31 = (a.Row3 * b.Column1).Sum();
        result.M32 = (a.Row3 * b.Column2).Sum();
        result.M33 = (a.Row3 * b.Column3).Sum();

        return result;
    }

    public static Vector4 operator *(Matrix4X4 a, Vector4 b)
    {
        return new Vector4(
            (a.Row0 * b).Sum(),
            (a.Row1 * b).Sum(),
            (a.Row2 * b).Sum(),
            (a.Row3 * b).Sum());
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4X4 CreateRotation(Quaternion quaternion)
    {
        var xx = quaternion.X * quaternion.X;
        var yy = quaternion.Y * quaternion.Y;
        var zz = quaternion.Z * quaternion.Z;

        var xy = quaternion.X * quaternion.Y;
        var wz = quaternion.Z * quaternion.W;
        var xz = quaternion.Z * quaternion.X;
        var wy = quaternion.Y * quaternion.W;
        var yz = quaternion.Y * quaternion.Z;
        var wx = quaternion.X * quaternion.W;

        var x = new Vector4(
            1.0f - 2.0f * (yy + zz),
            2.0f * (xy + wz),
            2.0f * (xz - wy),
            0
        );
        
        var y = new Vector4(
            2.0f * (xy - wz),
            1.0f - 2.0f * (zz + xx),
            2.0f * (yz + wx),
            0
        );
        
        var z = new Vector4(
            2.0f * (xz + wy),
            2.0f * (yz - wx),
            1.0f - 2.0f * (yy + xx),
            0
        );
        
        return new Matrix4X4(x, y, z, Vector4.UnitW);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4X4 CreateRotation(Vector3 direction, float angle)
    {
        var cos = MathF.Cos(-angle);
        var sin = MathF.Sin(-angle);
        var t = 1.0f - cos;
        
        direction = direction.Normalized;

        return new Matrix4X4(
            t * direction.X * direction.X + cos,
            t * direction.X * direction.Y - sin * direction.Z,
            t * direction.X * direction.Z + sin * direction.Y,
            0,
            t * direction.X * direction.Y + sin * direction.Z,
            t * direction.Y * direction.Y + cos,
            t * direction.Y * direction.Z - sin * direction.X,
            0,
            t * direction.X * direction.Z - sin * direction.Y,
            t * direction.Y * direction.Z + sin * direction.X,
            t * direction.Z * direction.Z + cos,
            0,
            0,
            0,
            0,
            1
        );
    }

    /// <summary>
    /// Creating rotation axis X matrix
    /// <code>
    ///   1  |   0  |  0  |  0 
    ///   0  |  cos | sin |  0
    ///   0  | -sin | cos |  0
    ///   0  |   0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4X4 CreateRotationX(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        return new Matrix4X4(
            Vector4.UnitX,
            new Vector4(0, cos, sin, 0),
            new Vector4(0, -sin, cos, 0),
            Vector4.UnitW
        );
    }
    
    /// <summary>
    /// Creating rotation axis Y matrix
    /// <code>
    ///  cos |  0  | -sin  |  0 
    ///   0  |  1  |   0   |  0
    ///  sin |  0  |  cos  |  0
    ///   0  |  0  |   0   |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4X4 CreateRotationY(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        return new Matrix4X4(
            new Vector4(cos, 0, -sin, 0),
            Vector4.UnitY,
            new Vector4(sin, 0, cos, 0),
            Vector4.UnitW
        );
    }
    
    /// <summary>
    /// Creating rotation axis Z matrix
    /// <code>
    ///  cos | sin |  0  |  0 
    /// -sin | cos |  0  |  0
    ///   0  |  0  |  1  |  0
    ///   0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4X4 CreateRotationZ(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        return new Matrix4X4(
            new Vector4(cos, sin, 0, 0),
            new Vector4(-sin, cos, 0, 0),
            Vector4.UnitZ,
            Vector4.UnitW
        );
    }

    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  v 
    ///  0  |  1  |  0  |  v
    ///  0  |  0  |  1  |  v
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4X4 CreateTranslation(float value)
    {
        return CreateTranslation(value, value, value);
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
    public static Matrix4X4 CreateTranslation(Vector2 vector2)
    {
        return CreateTranslation(vector2.X, vector2.Y, 0f);
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
    public static Matrix4X4 CreateTranslation(Vector3 vector3)
    {
        return CreateTranslation(vector3.X, vector3.Y, vector3.Z);
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
    public static Matrix4X4 CreateTranslation(float x, float y, float z)
    {
        var result = Identity;

        result.M03 = x;
        result.M13 = y;
        result.M23 = z;

        return result;
    }

    public static Matrix4X4 CreateOrthographic(Box2 box2, float zNear, float zFar)
    {
        return CreateOrthographic(box2.Width, box2.Height, zNear, zFar);
    }
    
    public static Matrix4X4 CreateOrthographic(Vector2 size, float zNear, float zFar)
    {
        return CreateOrthographic(size.X, size.Y, zNear, zFar);
    }
    
    public static Matrix4X4 CreateOrthographic(float width, float height, float zNear, float zFar)
    {
        var result = Identity;
        var range = 1.0f / (zNear - zFar);
        
        result.Row0 = new Vector4(2.0f / width, 0, 0, 0);
        result.Row1 = new Vector4(0, 2.0f / height, 0, 0);
        result.Row2 = new Vector4(0, 0, range, 0);
        result.Row3 = new Vector4(0, 0, range * zNear, 1);
        
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

        var height = 1.0f / MathF.Tan(fov * 0.5f);
        var width = height / aspect;
        var range = float.IsPositiveInfinity(zFar) ? -1.0f : zFar / (zNear - zFar);
        
        result.Row0 = new Vector4(width, 0, 0, 0);
        result.Row1 = new Vector4(0, height, 0, 0);
        result.Row2 = new Vector4(0, 0, range, -1.0f);
        result.Row3 = new Vector4(0, 0, range * zNear, 0);

        return result;
    }
}

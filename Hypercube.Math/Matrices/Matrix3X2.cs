using System.Runtime.CompilerServices;
using Hypercube.Math.Vectors;

namespace Hypercube.Math.Matrices;

public partial struct Matrix3X2
{
    public static Matrix3X2 Zero => new(Vector2.Zero);
    public static Matrix3X2 One => new(Vector2.One);
    public static Matrix3X2 Identity => new(Vector2.UnitX, Vector2.UnitY, Vector2.Zero);

    public Vector2 Row0;
    public Vector2 Row1;
    public Vector2 Row2;

    public Vector3 Column0 => new(M00, M10, M20);
    public Vector3 Column1 => new(M01, M11, M21);

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

    public Matrix3X2(float m00, float m01, float m10, float m11, float m20, float m21)
    {
        Row0 = new Vector2(m00, m01);
        Row1 = new Vector2(m10, m11);
        Row2 = new Vector2(m20, m21);
    }

    public Matrix3X2(Vector2 vector)
    {
        Row0 = vector;
        Row1 = vector;
        Row2 = vector;
    }

    public Matrix3X2(Vector2 row0, Vector2 row1, Vector2 row2)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
    }

    /// <summary>
    /// Creating rotation matrix
    /// <code>
    /// cos | -sin
    /// sin |  cos
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3X2 CreateRotation(Angle angle)
    {
        return CreateRotation(angle.Theta);
    }

    /// <summary>
    /// Creating rotation matrix
    /// <code>
    /// cos | -sin
    /// sin |  cos
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3X2 CreateRotation(double angle)
    {
        var cos = (float)System.Math.Cos(angle);
        var sin = (float)System.Math.Sin(angle);

        return new Matrix3X2(cos, sin, -sin, cos, 0, 0);
    }
    
    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |   0
    ///  0  |   y
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3X2 CreateScale(Vector2 scale)
    {
        return CreateScale(scale.X, scale.Y);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |   0
    ///  0  |   y
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3X2 CreateScale(float x, float y)
    {
        return new Matrix3X2(x, 0, y, 0, 0, 0);
    }
}
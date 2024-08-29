using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
public readonly partial struct Vector4 : IEquatable<Vector4>, IComparable<Vector4>, IComparable<float>, IEnumerable<float>
{
    /// <value>
    /// Vector (float.NaN, float.NaN, float.NaN, float.NaN).
    /// </value>
    public static readonly Vector4 NaN = new(float.NaN);
    
    /// <value>
    /// Vector (float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).
    /// </value>
    public static readonly Vector4 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <value>
    /// Vector (float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).
    /// </value>
    public static readonly Vector4 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <value>
    /// Vector (0, 0, 0, 0).
    /// </value>
    public static readonly Vector4 Zero = new(0);
    
    /// <value>
    /// Vector (1, 1, 1, 1).
    /// </value>
    public static readonly Vector4 One = new(1);
    
    /// <value>
    /// Vector (1, 0, 0, 0).
    /// </value>
    public static readonly Vector4 UnitX = new(1, 0, 0, 0);
    
    /// <value>
    /// Vector (0, 1, 0, 0).
    /// </value>
    public static readonly Vector4 UnitY = new(0, 1, 0, 0);
    
    /// <value>
    /// Vector (0, 0, 1, 0).
    /// </value>
    public static readonly Vector4 UnitZ = new(0, 0, 1, 0);
    
    /// <value>
    /// Vector (0, 0, 0, 1).
    /// </value>
    public static readonly Vector4 UnitW = new(0, 0, 0, 1);
    
    /// <summary>
    /// Vector X component.
    /// </summary>
    public readonly float X;
    
    /// <summary>
    /// Vector Y component.
    /// </summary>
    public readonly float Y;
    
    /// <summary>
    /// Vector Z component.
    /// </summary>
    public readonly float Z;
    
    /// <summary>
    /// Vector W component.
    /// </summary>
    public readonly float W;

    public Vector2 XY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y);
    }

    public Vector3 XYZ
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y, Z);
    }

    /// <summary>
    /// Gets the square of the vector length (magnitude).
    /// </summary>
    /// <remarks>
    /// Allows you to avoid using the rather expensive sqrt operation.
    /// (On ARM64 hardware <see cref="Length"/> may use the FRSQRTE instruction, which would take away this advantage).
    /// </remarks>
    /// <seealso cref="Length"/>
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z + W * W;
    }
    
    /// <summary>
    /// Gets the length (magnitude) of the vector.
    /// </summary>
    /// <remarks>
    /// On ARM64 hardware this may use the FRSQRTE instruction
    /// which performs a single Newton-Raphson iteration.
    /// On hardware without specialized support
    /// sqrt is used, which makes the method less fast.
    /// </remarks>
    /// <seealso cref="LengthSquared"/>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / MathF.ReciprocalSqrtEstimate(LengthSquared);
    }

    /// <summary>
    /// Copy of scaled to unit length.
    /// </summary>
    public Vector4 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }
    
    /// <summary>
    /// Summation of all vector components.
    /// </summary>
    public float Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y + Z + W;
    }

    /// <summary>
    /// Production of all vector components.
    /// </summary>
    public float Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z * W;
    }
    
    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                3 => W,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    public Vector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    
    public Vector4(float value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    public Vector4(double x, double y, double z, double w)
    {
        X = (float) x;
        Y = (float) y;
        Z = (float) z;
        W = (float) w;
    }
    
    public Vector4(double value)
    {
        X = (float) value;
        Y = (float) value;
        Z = (float) value;
        W = (float) value;
    }
    
    public Vector4(Vector2 value, float z, float w)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
        W = w;
    }
    
    public Vector4(Vector3 value, float w)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = w;
    }

    public Vector4(Vector4 value)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = value.W;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithX(float value)
    {
        return new Vector4(value, Y, Z, W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithY(float value)
    {
        return new Vector4(X, value, Z, W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithZ(float value)
    {
        return new Vector4(X, Y, value, W);
    }    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithW(float value)
    {
        return new Vector4(X, Y, Z, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector4 other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(float other)
    {
        return LengthSquared.CompareTo(other * other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<float> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
        yield return W;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector4 other)
    {
        return X.AboutEquals(other.X) &&
               Y.AboutEquals(other.Y) &&
               Z.AboutEquals(other.Z) &&
               W.AboutEquals(other.W);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector4 other && Equals(other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{X}, {Y}, {Z}, {W}";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
        return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, Vector3 b)
    {
        return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, Vector2 b)
    {
        return new Vector4(a.X + b.X, a.Y + b.Y, a.Z, a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, float b)
    {
        return new Vector4(a.X + b, a.Y + b, a.Z + b, a.W + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a)
    {
        return new Vector4(-a.X, -a.Y, -a.Z, -a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, Vector4 b)
    {
        return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, Vector3 b)
    {
        return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, Vector2 b)
    {
        return new Vector4(a.X - b.X, a.Y - b.Y, a.Z, a.W);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, float b)
    {
        return new Vector4(a.X - b, a.Y - b, a.Z - b, a.W - b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 a, Vector4 b)
    {
        return new Vector4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 a, Vector3 b)
    {
        return new Vector4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 a, Vector2 b)
    {
        return new Vector4(a.X * b.X, a.Y * b.Y, a.Z, a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 a, float b)
    {
        return new Vector4(a.X * b, a.Y * b, a.Z * b, a.W * b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 a, Vector4 b)
    {
        return new Vector4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 a, Vector3 b)
    {
        return new Vector4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 a, Vector2 b)
    {
        return new Vector4(a.X / b.X, a.Y / b.Y, a.Z, a.W);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 a, float b)
    {
        return new Vector4(a.X / b, a.Y / b, a.Z / b, a.W / b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4 a, Vector4 b)
    {
        return a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector4 a, Vector4 b)
    {
        return !a.Equals(b);
    }
}
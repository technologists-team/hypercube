using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Represents a vector with three single-precision floating-point values.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential), DebuggerDisplay("({X}, {Y}, {Z})")]
public readonly partial struct Vector3 : IEquatable<Vector3>, IComparable<Vector3>, IComparable<float>, IEnumerable<float>
{
    /// <value>
    /// Vector (float.NaN, float.NaN, float.NaN).
    /// </value>
    public static readonly Vector3 NaN = new(float.NaN);
    
    /// <value>
    /// Vector (float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).
    /// </value>
    public static readonly Vector3 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <value>
    /// Vector (float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).
    /// </value>
    public static readonly Vector3 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <value>
    /// Vector (0, 0, 0).
    /// </value>
    public static readonly Vector3 Zero = new(0, 0, 0);
    
    /// <value>
    /// Vector (1, 1, 1).
    /// </value>
    public static readonly Vector3 One = new(1, 1, 1);

    /// <value>
    /// Vector (1, 0, 0).
    /// </value>
    public static readonly Vector3 UnitX = new(1, 0, 0);
    
    /// <value>
    /// Vector (0, 1, 0).
    /// </value>
    public static readonly Vector3 UnitY = new(0, 1, 0);
    
    /// <value>
    /// Vector (0, 0, 1).
    /// </value>
    public static readonly Vector3 UnitZ = new(0, 0, 1);
    
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
        get => X * X + Y * Y + Z * Z;
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
    public Vector3 Normalized
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
        get => X + Y + Z;
    }

    /// <summary>
    /// Production of all vector components.
    /// </summary>
    public float Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z;
    }

    public Vector2 XY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y);
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
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vector3(float value)
    {
        X = value;
        Y = value;
        Z = value;
    }
    
    public Vector3(double x, double y, double z)
    {
        X = (float) x;
        Y = (float) y;
        Z = (float) z;
    }
    
    public Vector3(double value)
    {
        X = (float) value;
        Y = (float) value;
        Z = (float) value;
    }
    
    public Vector3(Vector2 value)
    {
        X = value.X;
        Y = value.Y;
        Z = 0;
    }
    
    public Vector3(Vector2 value, float z)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
    }

    public Vector3(Vector3 value)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithX(float value)
    {
        return new Vector3(value, Y, Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithY(float value)
    {
        return new Vector3(X, value, Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithZ(float value)
    {
        return new Vector3(X, Y, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float DistanceSquared(Vector3 value)
    {
        return DistanceSquared(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Distance(Vector3 value)
    {
        return Distance(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Cross(Vector3 value)
    {
        return Cross(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Dot(Vector3 value)
    {
        return Dot(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Reflect(Vector3 normal)
    {
        return Reflect(this, normal);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 MoveTowards(Vector3 target, float distance)
    {
        return MoveTowards(this, target, distance);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Clamp(Vector3 min, Vector3 max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Clamp(float min, float max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Lerp(Vector3 value, float amount)
    {
        return Lerp(this, value, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Max(Vector3 value)
    {
        return Max(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Min(Vector3 value)
    {
        return Min(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Abs()
    {
        return Abs(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Round()
    {
        return Round(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Round(int digits)
    {
        return Round(this, digits);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Ceiling()
    {
        return Ceiling(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Floor()
    {
        return Floor(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector3 other)
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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3 other)
    {
        return X.AboutEquals(other.X) &&
               Y.AboutEquals(other.Y) &&
               Z.AboutEquals(other.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector3 other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{X}, {Y}, {Z}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, Vector2 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, float b)
    {
        return new Vector3(a.X + b, a.Y + b, a.Z + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(float a, Vector3 b)
    {
        return new Vector3(a + b.X, a + b.Y, a + b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a)
    {
        return new Vector3(-a.X, -a.Y, -a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, Vector2 b)
    {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, float b)
    {
        return new Vector3(a.X - b, a.Y - b, a.Z - b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(float a, Vector3 b)
    {
        return new Vector3(a - b.X, a - b.Y, a - b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, Vector2 b)
    {
        return new Vector3(a.X * b.X, a.Y * b.Y, a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, float b)
    {
        return new Vector3(a.X * b, a.Y * b, a.Z * b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(float a, Vector3 b)
    {
        return new Vector3(a * b.X, a * b.Y, a * b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, Vector2 b)
    {
        return new Vector3(a.X / b.X, a.Y / b.Y, a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, float b)
    {
        return new Vector3(a.X / b, a.Y / b, a.Z / b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(float a, Vector3 b)
    {
        return new Vector3(a / b.X, a / b.Y, a / b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3 a, Vector3 b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3 a, Vector3 b)
    {
        return !a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2 valueA, Vector2 valueB)
    {
        return (valueA - valueB).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2 valueA, Vector2 valueB)
    {
        return (valueA - valueB).Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Cross(Vector3 valueA, Vector3 valueB)
    {
        return new Vector3(
            valueA.Y * valueB.Z - valueA.Z * valueB.Y,
            valueA.Z * valueB.X - valueA.X * valueB.Z,
            valueA.X * valueB.Y - valueA.Y * valueB.X);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector3 valueA, Vector3 valueB)
    {
        return valueA.X * valueB.X + valueA.Y * valueB.Y + valueA.Z * valueB.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Reflect(Vector3 value, Vector3 normal)
    {
        return value - 2.0f * (Dot(value, normal) * normal);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 MoveTowards(Vector3 current, Vector3 target, float distance)
    {
        return new Vector3(
            HyperMathF.MoveTowards(current.X, target.X, distance),
            HyperMathF.MoveTowards(current.Y, target.Y, distance),
            HyperMathF.MoveTowards(current.Z, target.Z, distance));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
        return new Vector3(
            float.Clamp(value.X, min.X, max.X),
            float.Clamp(value.Y, min.Y, max.Y),
            float.Clamp(value.Z, min.Z, max.Z));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(Vector3 value, float min, float max)
    {
        return new Vector3(
            float.Clamp(value.X, min, max),
            float.Clamp(value.Y, min, max),
            float.Clamp(value.Z, min, max));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Lerp(Vector3 value, Vector3 target, float amount)
    {
        return new Vector3(
            float.Lerp(value.X, target.X, amount),
            float.Lerp(value.Y, target.Y, amount),
            float.Lerp(value.Z, target.Z, amount));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Max(Vector3 valueA, Vector3 valueB)
    {
        return new Vector3(
            MathF.Max(valueA.X, valueB.X),
            MathF.Max(valueA.Y, valueB.Y),
            MathF.Max(valueA.Z, valueB.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Min(Vector3 valueA, Vector3 valueB)
    {
        return new Vector3(
            MathF.Min(valueA.X, valueB.X),
            MathF.Min(valueA.Y, valueB.Y),
            MathF.Min(valueA.Z, valueB.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Abs(Vector3 value)
    {
        return new Vector3(
            Math.Abs(value.X),
            Math.Abs(value.Y),
            Math.Abs(value.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Round(Vector3 value)
    {
        return new Vector3(
            Math.Round(value.X),
            Math.Round(value.Y),
            Math.Round(value.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Round(Vector3 value, int digits)
    {
        return new Vector3(
            Math.Round(value.X, digits),
            Math.Round(value.Y, digits),
            Math.Round(value.Z, digits));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Ceiling(Vector3 value)
    {
        return new Vector3(
            Math.Ceiling(value.X),
            Math.Ceiling(value.Y),
            Math.Ceiling(value.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Floor(Vector3 value)
    {
        return new Vector3(
            Math.Floor(value.X),
            Math.Floor(value.Y),
            Math.Floor(value.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Sign(Vector3 value)
    {
        return new Vector3(
            Math.Sign(value.X),
            Math.Sign(value.Y),
            Math.Sign(value.Z));
    }
}
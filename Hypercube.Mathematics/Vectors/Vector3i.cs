using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential), DebuggerDisplay("({X}, {Y}, {Z})")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly partial struct Vector3i : IEquatable<Vector3i>, IComparable<Vector3i>, IComparable<int>, IEnumerable<int>
{
    /// <value>
    /// Vector (0, 0, 0).
    /// </value>
    public static readonly Vector3i Zero = new(0, 0, 0);
    
    /// <value>
    /// Vector (1, 1, 1).
    /// </value>
    public static readonly Vector3i One = new(1, 1, 1);

    /// <value>
    /// Vector (1, 0, 0).
    /// </value>
    public static readonly Vector3i UnitX = new(1, 0, 0);
    
    /// <value>
    /// Vector (0, 1, 0).
    /// </value>
    public static readonly Vector3i UnitY = new(0, 1, 0);
    
    /// <value>
    /// Vector (0, 0, 1).
    /// </value>
    public static readonly Vector3i UnitZ = new(0, 0, 1);
    
    /// <summary>
    /// Vector X component.
    /// </summary>
    public readonly int X;
    
    /// <summary>
    /// Vector Y component.
    /// </summary>
    public readonly int Y;
    
    /// <summary>
    /// Vector Z component.
    /// </summary>
    public readonly int Z;
    
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
    /// Summation of all vector components.
    /// </summary>
    public int Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y + Z;
    }

    /// <summary>
    /// Production of all vector components.
    /// </summary>
    public int Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z;
    }

    public Vector2i XY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y);
    }
    
    public int this[int index]
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
    
    public Vector3i(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vector3i(int value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    public Vector3i(Vector2i value)
    {
        X = value.X;
        Y = value.Y;
        Z = 0;
    }
    
    public Vector3i(Vector2i value, int z)
    {
        X = value.X;
        Y = value.Y;
        Z = z;
    }

    public Vector3i(Vector3i value)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i WithX(int value)
    {
        return new Vector3i(value, Y, Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i WithY(int value)
    {
        return new Vector3i(X, value, Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i WithZ(int value)
    {
        return new Vector3i(X, Y, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float DistanceSquared(Vector3i value)
    {
        return DistanceSquared(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Distance(Vector3i value)
    {
        return Distance(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Cross(Vector3i value)
    {
        return Cross(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Dot(Vector3i value)
    {
        return Dot(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Reflect(Vector3i normal)
    {
        return Reflect(this, normal);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i MoveTowards(Vector3i target, int distance)
    {
        return MoveTowards(this, target, distance);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Clamp(Vector3i min, Vector3i max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Clamp(int min, int max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Max(Vector3i value)
    {
        return Max(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Min(Vector3i value)
    {
        return Min(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Abs()
    {
        return Abs(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i Sign()
    {
        return Sign(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(int other)
    {
        return LengthSquared.CompareTo(other * other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector3i other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<int> GetEnumerator()
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
    public bool Equals(Vector3i other)
    {
        return X == other.X &&
               Y == other.Y &&
               Z == other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector3i other && Equals(other);
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
    public static Vector3i operator +(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(Vector3i a, Vector2i b)
    {
        return new Vector3i(a.X + b.X, a.Y + b.Y, a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(Vector3i a, int b)
    {
        return new Vector3i(a.X + b, a.Y + b, a.Z + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a)
    {
        return new Vector3i(-a.X, -a.Y, -a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a, Vector2i b)
    {
        return new Vector3i(a.X - b.X, a.Y - b.Y, a.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a, int b)
    {
        return new Vector3i(a.X - b, a.Y - b, a.Z - b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(Vector3i a, Vector2i b)
    {
        return new Vector3i(a.X * b.X, a.Y * b.Y, a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(Vector3i a, int b)
    {
        return new Vector3i(a.X * b, a.Y * b, a.Z * b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(int a, Vector3i b)
    {
        return new Vector3i(a * b.X, a * b.Y, a * b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator /(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator /(Vector3i a, Vector2i b)
    {
        return new Vector3i(a.X / b.X, a.Y / b.Y, a.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator /(Vector3i a, int b)
    {
        return new Vector3i(a.X / b, a.Y / b, a.Z / b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator /(int a, Vector3i b)
    {
        return new Vector3i(a / b.X, a / b.Y, a / b.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3i a, Vector3i b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3i a, Vector3i b)
    {
        return !a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector3i valueA, Vector3i valueB)
    {
        return (valueA - valueB).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector3i valueA, Vector3i valueB)
    {
        return (valueA - valueB).Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Cross(Vector3i valueA, Vector3i valueB)
    {
        return new Vector3i(
            valueA.Y * valueB.Z - valueA.Z * valueB.Y,
            valueA.Z * valueB.X - valueA.X * valueB.Z,
            valueA.X * valueB.Y - valueA.Y * valueB.X);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(Vector3i valueA, Vector3i valueB)
    {
        return valueA.X * valueB.X + valueA.Y * valueB.Y + valueA.Z * valueB.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Reflect(Vector3i value, Vector3i normal)
    {
        return value - 2 * (Dot(value, normal) * normal);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i MoveTowards(Vector3i current, Vector3i target, int distance)
    {
        return new Vector3i(
            HyperMath.MoveTowards(current.X, target.X, distance),
            HyperMath.MoveTowards(current.Y, target.Y, distance),
            HyperMath.MoveTowards(current.Z, target.Z, distance));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Clamp(Vector3i value, Vector3i min, Vector3i max)
    {
        return new Vector3i(
            int.Clamp(value.X, min.X, max.X),
            int.Clamp(value.Y, min.Y, max.Y),
            int.Clamp(value.Z, min.Z, max.Z));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Clamp(Vector3i value, int min, int max)
    {
        return new Vector3i(
            int.Clamp(value.X, min, max),
            int.Clamp(value.Y, min, max),
            int.Clamp(value.Z, min, max));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Max(Vector3i valueA, Vector3i valueB)
    {
        return new Vector3i(
            Math.Max(valueA.X, valueB.X),
            Math.Max(valueA.Y, valueB.Y),
            Math.Max(valueA.Z, valueB.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Min(Vector3i valueA, Vector3i valueB)
    {
        return new Vector3i(
            Math.Min(valueA.X, valueB.X),
            Math.Min(valueA.Y, valueB.Y),
            Math.Min(valueA.Z, valueB.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Abs(Vector3i value)
    {
        return new Vector3i(
            Math.Abs(value.X),
            Math.Abs(value.Y),
            Math.Abs(value.Z));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Sign(Vector3i value)
    {
        return new Vector3i(
            Math.Sign(value.X),
            Math.Sign(value.Y),
            Math.Sign(value.Z));
    }
}
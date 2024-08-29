using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential), DebuggerDisplay("({X}, {Y})")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly partial struct Vector2i : IEquatable<Vector2i>, IComparable<Vector2i>, IComparable<int>, IEnumerable<int>
{
    /// <value>
    /// Vector (0, 0).
    /// </value>
    public static readonly Vector2i Zero = new(0);
    
    /// <value>
    /// Vector (1, 1).
    /// </value>
    public static readonly Vector2i One = new(1);

    /// <value>
    /// Vector (1, 0).
    /// </value>
    public static readonly Vector2i UnitX = new(1, 0);
    
    /// <value>
    /// Vector (0, 1).
    /// </value>
    public static readonly Vector2i UnitY = new(0, 1);
    
    /// <summary>
    /// Vector X component.
    /// </summary>
    public readonly int X;
    
    /// <summary>
    /// Vector Y component.
    /// </summary>
    public readonly int Y;
    
    public float AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X / (float) Y;
    }
    
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y;
    }
    
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / MathF.ReciprocalSqrtEstimate(LengthSquared);
    }
    
    public Vector2i Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }
    
    public float Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Atan2(Y, X);
    }

    public int Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y;
    }

    public int Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
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
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2i(int value)
    {
        X = value;
        Y = value;
    }
    
    public Vector2i(float x, float y)
    {
        X = (int) x;
        Y = (int) y;
    }
    
    public Vector2i(float value)
    {
        X = (int) value;
        Y = (int) value;
    }
    
    public Vector2i(Vector2i value)
    {
        X = value.X;
        Y = value.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i WithX(int value)
    {
        return new Vector2i(value, Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i WithY(int value)
    {
        return new Vector2i(X, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float DistanceSquared(Vector2i value)
    {
        return DistanceSquared(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Distance(Vector2i value)
    {
        return Distance(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Dot(Vector2i value)
    {
        return Dot(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i Clamp(Vector2i min, Vector2i max)
    {
        return Clamp(this, min, max);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i Clamp(int min, int max)
    {
        return Clamp(this, min, max);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i Max(Vector2i value)
    {
        return Max(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i Min(Vector2i value)
    {
        return Min(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i Abs()
    {
        return Abs(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i MoveTowards(Vector2i target, int distance)
    {
        return MoveTowards(this, target, distance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i Rotate(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);
        return new Vector2i(
            cos * X - sin * Y,
            sin * X + cos * Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(int other)
    {
        return LengthSquared.CompareTo(other * other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2i other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<int> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2i other)
    {
        return X == other.X &&
               Y == other.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector2i vector && Equals(vector);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.X + b.X, a.Y + b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i a, int b)
    {
        return new Vector2i(a.X + b, a.Y + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a)
    {
        return new Vector2i(-a.X, -a.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.X - b.X, a.Y - b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a, int b)
    {
        return new Vector2i(a.X - b, a.Y - b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.X * b.X, a.Y * b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(Vector2i a, int b)
    {
        return new Vector2i(a.X * b, a.Y * b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(Vector2i a, float b)
    {
        return new Vector2i(a.X * b, a.Y * b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator /(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.X / b.X, a.Y / b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator /(Vector2i a, int b)
    {
        return new Vector2i(a.X / b, a.Y / b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator /(Vector2i a, float b)
    {
        return new Vector2i(a.X / b, a.Y / b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2i a, Vector2i b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2i a, Vector2i b)
    {
        return !a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2i a, Vector2i b)
    {
        return a.CompareTo(b) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2i a, Vector2i b)
    {
        return a.CompareTo(b) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2i a, Vector2i b)
    {
        return a.CompareTo(b) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2i a, Vector2i b)
    {
        return a.CompareTo(b) is 1 or 0;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2i a, int b)
    {
        return a.CompareTo(b) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2i a, int b)
    {
        return a.CompareTo(b) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2i a, int b)
    {
        return a.CompareTo(b) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2i a, int b)
    {
        return a.CompareTo(b) is 1 or 0;
    }
    
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2i valueA, Vector2i valueB)
    {
        return (valueA - valueB).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2i valueA, Vector2i valueB)
    {
        return (valueA - valueB).Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector2i valueA, Vector2i valueB)
    {
        return valueA.X * valueB.X + valueA.Y * valueB.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i MoveTowards(Vector2i current, Vector2i target, int distance)
    {
        return new Vector2i(
            HyperMathF.MoveTowards(current.X, target.X, distance),
            HyperMathF.MoveTowards(current.Y, target.Y, distance));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Clamp(Vector2i value, Vector2i min, Vector2i max)
    {
        return new Vector2i(
            int.Clamp(value.X, min.X, max.X),
            int.Clamp(value.Y, min.Y, max.Y));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Clamp(Vector2i value, int min, int max)
    {
        return new Vector2i(
            int.Clamp(value.X, min, max),
            int.Clamp(value.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Max(Vector2i valueA, Vector2i valueB)
    {
        return new Vector2i(
            MathF.Max(valueA.X, valueB.X),
            MathF.Max(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Min(Vector2i valueA, Vector2i valueB)
    {
        return new Vector2i(
            MathF.Min(valueA.X, valueB.X),
            MathF.Min(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Abs(Vector2i value)
    {
        return new Vector2i(
            Math.Abs(value.X),
            Math.Abs(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Sign(Vector2i value)
    {
        return new Vector2i(
            Math.Sign(value.X),
            Math.Sign(value.Y));
    }
}
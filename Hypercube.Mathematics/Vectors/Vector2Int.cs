using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, StructLayout(LayoutKind.Sequential)]
public readonly partial struct Vector2Int : IEquatable<Vector2Int>, IComparable<Vector2Int>, IComparable<int>
{
    public static readonly Vector2Int Zero = new(0, 0);
    public static readonly Vector2Int One = new(1, 1);

    public static readonly Vector2Int UnitX = new(1, 0);
    public static readonly Vector2Int UnitY = new(0, 1);
    
    public readonly int X;
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
        get => MathF.Sqrt(LengthSquared);
    }
    
    public Vector2Int Normalized
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

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2Int(int value)
    {
        X = value;
        Y = value;
    }
    
    public Vector2Int(float x, float y)
    {
        X = (int) x;
        Y = (int) y;
    }
    
    public Vector2Int(float value)
    {
        X = (int) value;
        Y = (int) value;
    }
    
    public Vector2Int(Vector2Int value)
    {
        X = value.X;
        Y = value.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int WithX(int value)
    {
        return new Vector2Int(value, Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int WithY(int value)
    {
        return new Vector2Int(X, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float DistanceSquared(Vector2Int value)
    {
        return (this - value).LengthSquared;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Distance(Vector2Int value)
    {
        return MathF.Sqrt(DistanceSquared(this, value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Dot(Vector2Int value)
    {
        return Dot(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Cross(Vector2Int value)
    {
        return Cross(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Clamp(Vector2Int min, Vector2Int max)
    {
        return Clamp(this, min, max);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Clamp(int min, int max)
    {
        return Clamp(this, min, max);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Max(Vector2Int value)
    {
        return Max(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Min(Vector2Int value)
    {
        return Min(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Abs()
    {
        return Abs(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int MoveTowards(Vector2Int target, int distance)
    {
        return MoveTowards(this, target, distance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int Rotate(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);
        return new Vector2Int(
            cos * X - sin * Y,
            sin * X + cos * Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(int other)
    {
        return LengthSquared.CompareTo(other * other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2Int other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2Int other)
    {
        return X == other.X &&
               Y == other.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector2Int vector && Equals(vector);
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
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator +(Vector2Int a, int b)
    {
        return new Vector2Int(a.X + b, a.Y + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator -(Vector2Int a)
    {
        return new Vector2Int(-a.X, -a.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator -(Vector2Int a, int b)
    {
        return new Vector2Int(a.X - b, a.Y - b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X * b.X, a.Y * b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.X * b, a.Y * b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator *(Vector2Int a, float b)
    {
        return new Vector2Int(a.X * b, a.Y * b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator /(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X / b.X, a.Y / b.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator /(Vector2Int a, int b)
    {
        return new Vector2Int(a.X / b, a.Y / b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator /(Vector2Int a, float b)
    {
        return new Vector2Int(a.X / b, a.Y / b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2Int a, Vector2Int b)
    {
        return a.CompareTo(b) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2Int a, Vector2Int b)
    {
        return a.CompareTo(b) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2Int a, Vector2Int b)
    {
        return a.CompareTo(b) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2Int a, Vector2Int b)
    {
        return a.CompareTo(b) is 1 or 0;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2Int a, int b)
    {
        return a.CompareTo(b) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2Int a, int b)
    {
        return a.CompareTo(b) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2Int a, int b)
    {
        return a.CompareTo(b) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2Int a, int b)
    {
        return a.CompareTo(b) is 1 or 0;
    }
    
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2Int valueA, Vector2Int valueB)
    {
        return (valueA - valueB).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2Int valueA, Vector2Int valueB)
    {
        return MathF.Sqrt(DistanceSquared(valueA, valueB));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector2Int valueA, Vector2Int valueB)
    {
        return valueA.X * valueB.X + valueA.Y * valueB.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cross(Vector2Int valueA, Vector2Int valueB)
    {
        return valueA.X * valueB.Y - valueA.Y * valueB.X;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int MoveTowards(Vector2Int current, Vector2Int target, int distance)
    {
        return new Vector2Int(
            HyperMathF.MoveTowards(current.X, target.X, distance),
            HyperMathF.MoveTowards(current.Y, target.Y, distance));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Clamp(Vector2Int value, Vector2Int min, Vector2Int max)
    {
        return new Vector2Int(
            int.Clamp(value.X, min.X, max.X),
            int.Clamp(value.Y, min.Y, max.Y));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Clamp(Vector2Int value, int min, int max)
    {
        return new Vector2Int(
            int.Clamp(value.X, min, max),
            int.Clamp(value.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Max(Vector2Int valueA, Vector2Int valueB)
    {
        return new Vector2Int(
            MathF.Max(valueA.X, valueB.X),
            MathF.Max(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Min(Vector2Int valueA, Vector2Int valueB)
    {
        return new Vector2Int(
            MathF.Min(valueA.X, valueB.X),
            MathF.Min(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Abs(Vector2Int value)
    {
        return new Vector2Int(
            Math.Abs(value.X),
            Math.Abs(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Sign(Vector2Int value)
    {
        return new Vector2Int(
            Math.Sign(value.X),
            Math.Sign(value.Y));
    }
}
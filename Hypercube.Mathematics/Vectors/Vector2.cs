using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, StructLayout(LayoutKind.Sequential)]
public readonly partial struct Vector2 : IEquatable<Vector2>, IComparable<Vector2>, IComparable<float>
{
    public static readonly Vector2 NaN = new(float.NaN, float.NaN);
    public static readonly Vector2 PositiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity);
    public static readonly Vector2 NegativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity);
    public static readonly Vector2 Zero = new(0, 0);
    public static readonly Vector2 One = new(1, 1);
    
    public static readonly Vector2 UnitX = new(1, 0);
    public static readonly Vector2 UnitY = new(0, 1);
    
    public readonly float X;
    public readonly float Y;

    public float AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X / Y;
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
    
    public Vector2 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public float Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Atan2(Y, X);
    }

    public float Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y;
    }

    public float Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
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
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2(float value)
    {
        X = value;
        Y = value;
    }
    
    public Vector2(double x, double y)
    {
        X = (float) x;
        Y = (float) y;
    }
    
    public Vector2(double value)
    {
        X = (float) value;
        Y = (float) value;
    }

    public Vector2(Vector2 vector2)
    {
        X = vector2.X;
        Y = vector2.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithX(float value)
    {
        return new Vector2(value, Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithY(float value)
    {
        return new Vector2(X, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float DistanceSquared(Vector2 other)
    {
        return (this - other).LengthSquared;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Distance(Vector2 other)
    {
        return MathF.Sqrt(DistanceSquared(this, other));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Cross(Vector2 other)
    {
        return Cross(this, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Dot(Vector2 other)
    {
        return Dot(this, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Max(Vector2 other)
    {
        return Max(this, other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Min(Vector2 other)
    {
        return Min(this, other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Abs()
    {
        return Abs(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Round()
    {
        return Round(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Round(int digits)
    {
        return Round(this, digits);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Ceiling()
    {
        return Ceiling(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Floor()
    {
        return Floor(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Rotate(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);
        return new Vector2(
            cos * X - sin * Y,
            sin * X + cos * Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2 other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(float other)
    {
        return LengthSquared.CompareTo(other * other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2 other)
    {
        return X.AboutEquals(other.X) &&
               Y.AboutEquals(other.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector2 other && Equals(other);
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
    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(float a, Vector2 b)
    {
        return new Vector2(b.X + a, b.Y + a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a, float b)
    {
        return new Vector2(a.X + b, a.Y + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a)
    {
        return new Vector2(-a.X, -a.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(float a, Vector2 b)
    {
        return new Vector2(b.X - a, b.Y - a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, float b)
    {
        return new Vector2(a.X - b, a.Y - b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X * b.X, a.Y * b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(float a, Vector2 b)
    {
        return new Vector2(b.X * a, b.Y * a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.X * b, a.Y * b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X / b.X, a.Y / b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(float a, Vector2 b)
    {
        return new Vector2(b.X / a, b.Y / a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(a.X / b, a.Y / b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2 a, Vector2 b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2 a, Vector2 b)
    {
        return !a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2 a, Vector2 b)
    {
        return (a - b).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2 a, Vector2 b)
    {
        return MathF.Sqrt(DistanceSquared(a, b));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Max(Vector2 a, Vector2 b)
    {
        return new Vector2(
            MathF.Max(a.X, b.X),
            MathF.Max(a.Y, b.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Min(Vector2 a, Vector2 b)
    {
        return new Vector2(
            MathF.Min(a.X, b.X),
            MathF.Min(a.Y, b.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(
            Math.Abs(vector.X),
            Math.Abs(vector.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(Vector2 vector)
    {
        return new Vector2(
            Math.Round(vector.X),
            Math.Round(vector.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(Vector2 vector, int digits)
    {
        return new Vector2(
            Math.Round(vector.X, digits),
            Math.Round(vector.Y, digits));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Ceiling(Vector2 vector)
    {
        return new Vector2(
            Math.Ceiling(vector.X),
            Math.Ceiling(vector.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Floor(Vector2 vector)
    {
        return new Vector2(
            Math.Floor(vector.X),
            Math.Floor(vector.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Sign(Vector2 vector)
    {
        return new Vector2(
            Math.Sign(vector.X),
            Math.Sign(vector.Y));
    }
}
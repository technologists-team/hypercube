using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hypercube.Shared.Math.Vector;

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Vector2(float x, float y) : IEquatable<Vector2>
{
    public static readonly Vector2 Zero = new(0, 0);
    public static readonly Vector2 One = new(1, 1);
    public static readonly Vector2 Up = new(0, 1);
    public static readonly Vector2 Down = new(0, -1);
    public static readonly Vector2 Right = new(1, 0);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 UnitX = new(1, 0);
    public static readonly Vector2 UnitY = new(0, 1);
    
    public readonly float X = x;
    public readonly float Y = y;

    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Sqrt(x * x + y * y);
    }
    
    public Vector2(float value) : this(value, value)
    {
    }

    public Vector2(Vector2 vector2) : this(vector2.X, vector2.Y)
    {
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
    public bool Equals(Vector2 other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
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
        return $"{x}, {y}";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
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
}
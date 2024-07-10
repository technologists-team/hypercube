using System.Runtime.CompilerServices;
using Hypercube.Shared.Math.Extensions;

namespace Hypercube.Shared.Math.Vector;

public readonly partial struct Vector4(float x, float y, float z, float w) : IEquatable<Vector4>
{
    public static readonly Vector4 Zero = new(0, 0, 0, 0);
    public static readonly Vector4 One = new(1, 1, 1, 1);
    public static readonly Vector4 UnitX = new(1, 0, 0, 0);
    public static readonly Vector4 UnitY = new(0, 1, 0, 0);
    public static readonly Vector4 UnitZ = new(0, 0, 1, 0);
    public static readonly Vector4 UnitW = new(0, 0, 0, 1);
    
    public readonly float X = x;
    public readonly float Y = y;
    public readonly float Z = z;
    public readonly float W = w;

    public Vector4(float value) : this(value, value, value, value)
    {
    }

    public Vector4(Vector2 vector2, float z, float w) : this(vector2.X, vector2.Y, z, w)
    {
    }

    public Vector4(Vector4 vector4, float w) : this(vector4.X, vector4.Y, vector4.Z, w)
    {
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Sum()
    {
        return X + Y + Z + W;
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
        return $"{x}, {y}, {z}, {w}";
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Math.Extensions;

namespace Hypercube.Math.Vector;

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Vector3 : IEquatable<Vector3>
{
    public static readonly Vector3 Zero = new(0, 0, 0);
    public static readonly Vector3 One = new(1, 1, 1);

    public static readonly Vector3 UnitX = new(1, 0, 0);
    public static readonly Vector3 UnitY = new(0, 1, 0);
    public static readonly Vector3 UnitZ = new(0, 0, 1);
    
    public readonly float X;
    public readonly float Y;
    public readonly float Z;

    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z;
    }
    
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Sqrt(LengthSquared);
    }

    public Vector3 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vector3(float value) : this(value, value, value)
    {
    }
    
    public Vector3(Vector2 vector2) : this(vector2.X, vector2.Y, 0)
    {
    }
    
    public Vector3(Vector2 vector2, float z) : this(vector2.X, vector2.Y, z)
    {
    }

    public Vector3(Vector3 vector3) : this(vector3.X, vector3.Y, vector3.Z)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Sum()
    {
        return X + Y + Z;
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
    public Vector3 Cross(Vector3 other)
    {
        return Vector3.Cross(this, other);
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
    public static Vector3 Cross(Vector3 left, Vector3 right)
    {
        return new Vector3(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X);
    }
}
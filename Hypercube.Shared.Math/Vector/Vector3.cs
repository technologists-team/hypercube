using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Vector;

public readonly struct Vector3(float x, float y, float z)
{
    public static readonly Vector3 Zero = new(0, 0, 0);
    public static readonly Vector3 One = new(1, 1, 1);
    public static readonly Vector3 Forward = new(0, 0, 1); 
    public static readonly Vector3 Back = new(0, 0, -1);
    public static readonly Vector3 Up = new(0, 1, 0);
    public static readonly Vector3 Down = new(0, -1, 0);
    public static readonly Vector3 Right = new(1, 0, 0);
    public static readonly Vector3 Left = new(-1, 0, 0);
    public static readonly Vector3 UnitX = new(1, 0, 0);
    public static readonly Vector3 UnitY = new(0, 1, 0);
    public static readonly Vector3 UnitZ = new(0, 0, 1);
    
    public readonly float X = x;
    public readonly float Y = y;
    public readonly float Z = z;

    public Vector3(float value) : this(value, value, value)
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
    public override string ToString()
    {
        return $"{x}, {y}, {z}";
    }
}
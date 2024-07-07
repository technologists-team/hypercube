using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Vector;

public readonly partial struct Vector2(float x, float y)
{
    public static readonly Shared.Math.Vector.Vector2 Zero = new(0, 0);
    public static readonly Shared.Math.Vector.Vector2 One = new(1, 1);
    public static readonly Shared.Math.Vector.Vector2 Up = new(0, 1);
    public static readonly Shared.Math.Vector.Vector2 Down = new(0, -1);
    public static readonly Shared.Math.Vector.Vector2 Right = new(1, 0);
    public static readonly Shared.Math.Vector.Vector2 Left = new(-1, 0);
    
    public readonly float X = x;
    public readonly float Y = y;
    public readonly float Length = MathF.Sqrt(x * x + y * y);

    public Vector2(float value) : this(value, value)
    {
    }

    public Vector2(Shared.Math.Vector.Vector2 vector) : this(vector.X, vector.Y)
    {
        
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator +(Shared.Math.Vector.Vector2 a, Shared.Math.Vector.Vector2 b)
    {
        return new Shared.Math.Vector.Vector2(a.X + b.X, a.Y + b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator +(Shared.Math.Vector.Vector2 a, float b)
    {
        return new Shared.Math.Vector.Vector2(a.X + b, a.Y + b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator -(Shared.Math.Vector.Vector2 a)
    {
        return new Shared.Math.Vector.Vector2(-a.X, -a.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator -(Shared.Math.Vector.Vector2 a, Shared.Math.Vector.Vector2 b)
    {
        return new Shared.Math.Vector.Vector2(a.X - b.X, a.Y - b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator -(Shared.Math.Vector.Vector2 a, float b)
    {
        return new Shared.Math.Vector.Vector2(a.X - b, a.Y - b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator *(Shared.Math.Vector.Vector2 a, Shared.Math.Vector.Vector2 b)
    {
        return new Shared.Math.Vector.Vector2(a.X * b.X, a.Y * b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator *(Shared.Math.Vector.Vector2 a, float b)
    {
        return new Shared.Math.Vector.Vector2(a.X * b, a.Y * b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator /(Shared.Math.Vector.Vector2 a, Shared.Math.Vector.Vector2 b)
    {
        return new Shared.Math.Vector.Vector2(a.X / b.X, a.Y / b.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Shared.Math.Vector.Vector2 operator /(Shared.Math.Vector.Vector2 a, float b)
    {
        return new Shared.Math.Vector.Vector2(a.X / b, a.Y / b);
    }
}
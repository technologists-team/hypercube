using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.Shapes;

public readonly struct Box2
{
    public static readonly Box2 NaN = new(Vector2.NaN, Vector2.NaN);
    public static readonly Box2 Zero = new(Vector2.Zero, Vector2.Zero);
    public static readonly Box2 UV = new(0.0f, 1.0f, 1.0f, 0.0f);
    public static readonly Box2 Centered = new(-0.5f, -0.5f, 0.5f, 0.5f);
    
    public float Left => Point0.X;
    public float Top => Point0.Y;
    public float Right => Point1.X;
    public float Bottom => Point1.Y;

    public Vector2 TopLeft => new(Left, Top);
    public Vector2 BottomLeft => new(Left, Bottom);
    public Vector2 TopRight => new(Right, Top);
    public Vector2 BottomRight => new(Right, Bottom);
    
    public float Width => MathF.Abs(Right - Left);
    public float Height => MathF.Abs(Bottom - Top);
    public Vector2 Size => new(Width, Height);
    public Vector2 Center => (Point0 + Point1) / 2;

    public readonly Vector2 Point0;
    public readonly Vector2 Point1;

    public Vector2 Diagonal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TopLeft - BottomRight;
    }
    
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Width * Width + Height * Height;
    }
    
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Sqrt(LengthSquared);
    }

    public Vector2[] Vertices
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new[]
        {
            TopLeft, TopRight, BottomRight, BottomLeft
        };
    }
    
    public Box2(Vector2 point0, Vector2 point1)
    {
        Point0 = point0;
        Point1 = point1;
    }

    public Box2(float left, float top, float right, float bottom)
    {
        Point0 = new Vector2(left, top);
        Point1 = new Vector2(right, bottom);
    }

    public Box2(float value)
    {
        Point0 = new Vector2(value);
        Point1 = new Vector2(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Box2 operator +(Box2 a, Box2 b)
    {
        return new Box2(a.Point0 + b.Point0, a.Point1 + b.Point1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Box2 operator +(Box2 a, Vector2 b)
    {
        return new Box2(a.Point0 + b.X, a.Point1 + b.Y);
    }
}
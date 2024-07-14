using System.Diagnostics.CodeAnalysis;
using Hypercube.Math.Vectors;

namespace Hypercube.Math.Boxs;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public readonly struct Box2
{
    public static readonly Box2 UV = new(0.0f, 1.0f, 1.0f, 0.0f);
    
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
    
    public readonly Vector2 Point0;
    public readonly Vector2 Point1;

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
}
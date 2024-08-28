using Hypercube.Mathematics.Vectors;

namespace Hypercube.Shared.Physics;

public sealed class Transform
{
    public readonly static Transform Zero = new Transform(0f, 0f, 0f);

    public readonly Vector2 Position;
    public readonly float Sin;
    public readonly float Cos;
    
    public Transform(Vector2 position, float angle)
    {
        Position = position;
        Sin = MathF.Sin(angle);
        Cos = MathF.Cos(angle);
    }

    public Transform(float x, float y, float angle)
    {
        Position = new Vector2(x, y);
        Sin = MathF.Sin(angle);
        Cos = MathF.Cos(angle);
    }
}
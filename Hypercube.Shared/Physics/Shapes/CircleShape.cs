using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class CircleShape : IShape
{
    public ShapeType Type => ShapeType.Circle;

    public float Radius { get; set; } = 0.5f;
    public Vector2 Position { get; set; } = Vector2.Zero;
}
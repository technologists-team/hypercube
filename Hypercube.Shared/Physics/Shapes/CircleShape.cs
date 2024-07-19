using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class CircleShape : IShape
{
    public ShapeType Type => ShapeType.Circle;

    public float Radius { get; set; }
    public Vector2 Position { get; set; }

    public CircleShape()
    {
        Radius = 0.5f;
        Position = Vector2.Zero;
    }
    
    public CircleShape(float radius)
    {
        Radius = radius;
    }
    
    public CircleShape(float radius, Vector2 position)
    {
        Radius = radius;
        Position = position;
    }
}
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class CircleShape : IShape
{
    public ShapeType Type => ShapeType.Circle;
    
    public float Radius { get; }
    public Vector2 Position { get; }

    public int VerticesCount => 0;
    public Vector2[] Vertices => Array.Empty<Vector2>();

    public CircleShape() : this(0.5f, Vector2.Zero)
    {
    }
    
    public CircleShape(float radius) : this(radius, Vector2.Zero)
    {
    }
    
    public CircleShape(float radius, Vector2 position)
    {
        Radius = radius;
        Position = position;
    }
    
    public Vector2[] GetVerticesTransformed(Vector2 position, float rotation)
    {
        return Array.Empty<Vector2>();
    }
}
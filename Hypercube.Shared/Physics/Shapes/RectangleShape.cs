using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class RectangleShape : IShape
{
    public ShapeType Type => ShapeType.Polygon;

    public float Radius => 0;
    public Vector2 Position { get; }
    public Vector2 Size { get; }
    
    public int VerticesCount => 4;
    public Vector2[] Vertices { get; }

    public RectangleShape() : this(Vector2.Zero, Vector2.One / 2f)
    {
    }
    
    public RectangleShape(Vector2 size) : this(Vector2.Zero, size)
    {
    }
    
    public RectangleShape(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
        Vertices = new Box2(position - size / 2, position + size / 2).Vertices;
    }

    public Vector2[] GetVerticesTransformed(Vector2 position, float rotation)
    {
        return new Box2(position + Position - Size / 2, position + Position + Size / 2).Vertices;
    }

    public Box2 ComputeAABB(Vector2 position, float rotation)
    {
        var transformed = GetVerticesTransformed(position, rotation);
        return new Box2(transformed[0], transformed[2]);
    }
}
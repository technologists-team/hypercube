using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class PolygonShape : IShape
{
    public ShapeType Type => ShapeType.Polygon;

    public float Radius => 0;
    public Vector2 Position { get; }

    public int VerticesCount => Vertices.Length;
    public Vector2[] Vertices { get; }

    public PolygonShape(Vector2[] vertices) : this(vertices, Vector2.Zero)
    {
    }
    
    public PolygonShape(Vector2[] vertices, Vector2 position)
    {
        Vertices = vertices;
        Position = position;
    }
    
    public Vector2[] GetVerticesTransformed(Vector2 position, float rotation)
    {
        var vertices = new Vector2[VerticesCount];
        for (var i = 0; i < VerticesCount; i++)
        {
            vertices[i] = position + Vertices[i];
        }

        return vertices;
    }
}
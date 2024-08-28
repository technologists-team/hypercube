using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

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

    public Box2 ComputeAABB(Vector2 position, float rotation)
    {
        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;
        
        var vertices = GetVerticesTransformed(position, rotation);
        foreach (var vertex in vertices)
        {
            if (vertex.X < minX)
                minX = vertex.X;
            
            if (vertex.X > maxX)
                maxX = vertex.X;
            
            if (vertex.Y < minY)
                minY = vertex.Y;
            
            if (vertex.Y > maxY)
                maxY = vertex.Y;
        }

        return new Box2(minX, minY, maxX, maxY);
    }
}
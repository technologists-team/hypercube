using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics.Shapes;

public sealed class PolygonShape : IShape
{
    public ShapeType Type => ShapeType.Polygon;
    
    public float Radius { get; set; }
    
    public int VertexCount => Vertices.Length;
    public Vector2[] Vertices { get; } = Array.Empty<Vector2>();
    
}
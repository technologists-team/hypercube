namespace Hypercube.Shared.Physics.Shapes;

public sealed class PolygonShape : IShape
{
    public ShapeType Type => ShapeType.Polygon;
    
    public float Radius { get; set; }
}
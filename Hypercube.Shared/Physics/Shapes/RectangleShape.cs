namespace Hypercube.Shared.Physics.Shapes;

public sealed class RectangleShape : IShape
{
    public ShapeType Type => ShapeType.Rectangle;
    
    public float Radius { get; set; }
}
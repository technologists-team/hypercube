namespace Hypercube.Shared.Physics.Shapes;

public sealed class CapsuleShape : IShape
{
    public ShapeType Type => ShapeType.Capsule;
    
    public float Radius { get; set; }
}
namespace Hypercube.Shared.Physics;

public interface IShape
{
    ShapeType Type { get; }
    float Radius { get; set; }
}
using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public interface IBody
{
    IShape Shape { get; set; }
    
    Vector2 Velocity { get; }
    Vector2 Position { get; }
    Vector2 PreviousPosition { get; }
    
    Circle ShapeCircle { get; }
    
    void Move(Vector2 position);
}
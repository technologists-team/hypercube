using Hypercube.Math.Vectors;
using Hypercube.Shared.Physics.Shapes;

namespace Hypercube.Shared.Physics;

public interface IBody
{
    CircleShape Shape { get; }
    
    Vector2 Velocity { get; }
    Vector2 Position { get; }
    Vector2 PreviousPosition { get; }
    
    void Move(Vector2 position);
}
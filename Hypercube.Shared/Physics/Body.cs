using Hypercube.Math.Vectors;
using Hypercube.Shared.Physics.Shapes;

namespace Hypercube.Shared.Physics;

public sealed class Body : IBody
{
    public CircleShape Shape { get; private set; } = new CircleShape();
    
    public Vector2 Velocity { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 PreviousPosition { get; private set; }
    
    public void Move(Vector2 position)
    {
        Position += position;
    }
}
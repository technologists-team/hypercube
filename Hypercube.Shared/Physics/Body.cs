using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Physics.Shapes;

namespace Hypercube.Shared.Physics;

public sealed class Body : IBody
{
    public BodyType Type { get; set; } = BodyType.Dynamic;
    public bool IsStatic => Type == BodyType.Static;
    
    public IShape Shape { get; set; } = new CircleShape();
    
    public Vector2 Velocity { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 PreviousPosition { get; private set; }

    public Circle ShapeCircle => ((CircleShape)Shape).Circle + Position;
    public Box2 ShapeBox2 => ((RectangleShape)Shape).Box2 + Position;

    public void Update(float deltaTime)
    {
        throw new NotImplementedException();
    }

    public void Move(Vector2 position)
    {
        Position += position;
    }
}
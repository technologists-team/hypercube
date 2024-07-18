using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public interface IBody
{
    BodyType Type { get; set; }
    bool IsStatic { get; }
    
    IShape Shape { get; set; }
    
    Vector2 Velocity { get; }
    Vector2 Position { get; }
    Vector2 PreviousPosition { get; }
    
    Circle ShapeCircle { get; }
    Box2 ShapeBox2 { get; }

    void Update(float deltaTime);
    void Move(Vector2 position);
}
using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Physics.Shapes;

namespace Hypercube.Shared.Entities.Systems.Physics;

public sealed class PhysicsComponent : Component, IBody
{
    // Caching
    public TransformComponent TransformComponent;
    public TransformSystem TransformSystem;

    public BodyType Type { get; set; } = BodyType.Dynamic;
    public bool IsStatic => Type == BodyType.Static;
    
    public IShape Shape { get; set; } = new CircleShape();
    public Vector2 Velocity { get; set; }

    public Vector2 Position
    {
        get => TransformComponent.Transform.Position;
        set => TransformSystem.SetPosition(TransformComponent, value);
    }
    
    public Vector2 PreviousPosition { get; private set; }
    public Circle ShapeCircle => ((CircleShape)Shape).Circle + Position;

    public Box2 ShapeBox2
    {
        get
        {
            var shape = ((RectangleShape)Shape);
            return new Box2(Position - shape.Position - shape.Size / 2, Position + shape.Position + shape.Size / 2);
        }   
    }

    public void Update(float deltaTime)
    {
        if (IsStatic)
            return;
        
        //this.linearVelocity += gravity * time;
        Position += Velocity * deltaTime;
    }

    
    public void Move(Vector2 position)
    {
        Position += position;
    }
}
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

    public Vector2 Position
    {
        get => TransformComponent.Transform.Position;
        set => TransformSystem.SetPosition(TransformComponent, value);
    }

    public Vector2 LinearVelocity { get; set; }
    
    public float Angle { get; }
    public float AngularVelocity { get; }
    
    public Vector2 Force { get; }
    
    public float Density { get; }
    
    public float Mass { get; }
    public float InvMass { get; }
    
    public float Inertia { get; }
    public float InvInertia { get; }
    
    public float Restitution { get; }
    
    public float Friction { get; }
    
    public Circle ShapeCircle
    {
        get
        {
            var shape = (CircleShape)Shape;
            return new Circle(Position + shape.Position, shape.Radius);
        }
    }

    public Box2 ShapeBox2
    {
        get
        {
            var shape = (RectangleShape)Shape;
            return new Box2(Position - shape.Position - shape.Size / 2, Position + shape.Position + shape.Size / 2);
        }   
    }

    public void Update(float deltaTime)
    {
        if (IsStatic)
            return;
        
        //this.linearVelocity += gravity * time;
        Position += LinearVelocity * deltaTime;
    }

    
    public void Move(Vector2 position)
    {
        Position += position;
    }
}
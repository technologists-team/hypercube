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
    
    public float Angle { get; set; }
    public float AngularVelocity { get; }
    
    public Vector2 Force { get; set; }

    public float Density
    {
        get => _density;
        set => _density = value;
    }

    public float Mass
    {
        get => _mass; 
        set => _mass = value;
    }
    
    public float InvMass => IsStatic ? 0 : 1f / Mass;
    
    public float Inertia { get; }
    public float InvInertia { get; }
    
    public float Restitution
    {
        get => _restitution;
        set => _restitution = value;
    }
    
    public float Friction { get; }

    private float _density = 2f;
    private float _mass = 8f;
    private float _restitution = 0.5f;
    
    public Vector2[] GetShapeVerticesTransformed()
    {
        return Shape.GetVerticesTransformed(Position, Angle);
    }

    public void Update(float deltaTime)
    {
        if (IsStatic)
            return;

        var acceleration = Force / Mass; 
        
        LinearVelocity += acceleration * deltaTime;
        
        Position += LinearVelocity * deltaTime;
        Angle += AngularVelocity * deltaTime;
        
        Force = Vector2.Zero;
    }

    
    public void Move(Vector2 position)
    {
        Position += position;
    }
}
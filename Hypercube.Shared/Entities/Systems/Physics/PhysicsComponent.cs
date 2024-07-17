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
    
    public IShape Shape { get; set; } = new CircleShape();
    
    public Vector2 Velocity { get; private set; }

    public Vector2 Position
    {
        get => TransformComponent.Transform.Position;
        set => TransformSystem.SetPosition(TransformComponent, value);
    }
    
    public Vector2 PreviousPosition { get; private set; }
    public Circle ShapeCircle => ((CircleShape)Shape).Circle + Position;

    public void Move(Vector2 position)
    {
        Position += position;
    }
}
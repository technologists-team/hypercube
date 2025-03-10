using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Core.Systems.Transform;

[RegisterComponent]
public sealed class TransformComponent : Component
{
    public Entity? Parent;
    
    public Vector2 LocalPosition;

    public Angle LocalRotation;

    public Vector2 LocalScale;
}
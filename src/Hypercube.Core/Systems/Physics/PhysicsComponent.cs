using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Components;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Core.Systems.Physics;

[RegisterComponent]
public class PhysicsComponent : Component
{
    public Vector2 LinearVelocity;
   
    public float AngularVelocity;
}
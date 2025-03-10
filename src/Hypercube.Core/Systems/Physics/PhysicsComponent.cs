using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Core.Systems.Physics;

[RegisterComponent]
public class PhysicsComponent : Component
{
    public Vector2 LinearVelocity;
   
    public float AngularVelocity;
}
using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Core.Systems.Transform;

[RegisterEntitySystem]
public sealed class TransformSystem : EntitySystem
{
    public void SetLocalPosition(Entity entity, Vector2 position)
    {
        if (!TryGetComponent<TransformComponent>(entity, out var component))
            return;

        component.LocalPosition = position;
    }
    
    public void SetLocalRotation(Entity entity, Angle rotation)
    {
        if (!TryGetComponent<TransformComponent>(entity, out var component))
            return;

        component.LocalRotation = rotation;
    }
    
    public void SetLocalScale(Entity entity, Vector2 scale)
    {
        if (!TryGetComponent<TransformComponent>(entity, out var component))
            return;

        component.LocalScale = scale;
    }
}
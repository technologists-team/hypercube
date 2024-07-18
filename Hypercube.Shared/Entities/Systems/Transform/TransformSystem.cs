using Hypercube.Math.Vectors;
using Hypercube.Shared.Entities.Realisation;
using Hypercube.Shared.Entities.Realisation.Systems;

namespace Hypercube.Shared.Entities.Systems.Transform;

public class TransformSystem : EntitySystem
{
    public void SetPosition(TransformComponent transform, Vector2 position)
    {
        transform.Transform.SetPosition(position);
    }
    
    public void SetPosition(Entity<TransformComponent?> entity, Vector2 position)
    {
        var transform = entity.Component ?? GetComponent<TransformComponent>(entity);
        transform.Transform.SetPosition(position);
    }
}
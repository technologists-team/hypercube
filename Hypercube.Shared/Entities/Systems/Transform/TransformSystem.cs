using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Entities.Systems.Transform;

public class TransformSystem : EntitySystem
{
    public Vector2 GetWorldPosition(TransformComponent transform)
    {
        return transform.LocalPosition;
    }
}
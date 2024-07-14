using Hypercube.Math.Transforms;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.Scenes;

namespace Hypercube.Shared.Entities.Systems.Transform;

public sealed class TransformComponent : Component
{
    public SceneId SceneId;
    public Transform2 Transform = new();

    public SceneCoordinates Coordinates => new(SceneId, Transform.Position);
}
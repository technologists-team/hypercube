using Hypercube.Math.Vector;
using Hypercube.Shared.Scenes;

namespace Hypercube.Shared.Entities.Systems.Transform.Coordinates;

public readonly struct SceneCoordinates(SceneId scene, Vector2 position)
{
    public static readonly SceneCoordinates Nullspace = new(SceneId.Nullspace, Vector2.Zero);

    public float X => position.X;
    public float Y => position.Y;
    
    public readonly SceneId Scene = scene;
    public readonly Vector2 Position = position;

    public override string ToString()
    {
        return $"Scene {Scene} ({Position})";
    }
}
using Hypercube.Mathematics.Vectors;
using Hypercube.Shared.Scenes;

namespace Hypercube.Shared.Entities.Systems.Transform.Coordinates;

public readonly struct SceneCoordinates(SceneId scene, Vector2 position)
{
    public static readonly SceneCoordinates Nullspace = new(SceneId.Nullspace, Vector2.Zero);

    public float X => Position.X;
    public float Y => Position.Y;
    
    public readonly SceneId Scene = scene;
    public readonly Vector2 Position = position;

    public override string ToString()
    {
        return $"Scene {Scene} ({Position})";
    }
}
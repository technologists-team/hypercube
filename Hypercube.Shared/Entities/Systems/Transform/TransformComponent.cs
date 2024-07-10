using Hypercube.Shared.Entities.Realisation;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Scenes;

namespace Hypercube.Shared.Entities.Systems.Transform;

public sealed class TransformComponent : Component
{
    public EntityUid? Parent;

    public SceneId SceneId = SceneId.Nullspace;
    
    public Angle LocalRotation = Angle.Zero;
    public Vector2 LocalPosition = Vector2.Zero;
    public Vector2 LocalScale = Vector2.One;

    public Matrix3X3 LocalMatrix => Parent?.Valid ?? false
        ? Matrix3X3.CreateTransform(LocalPosition, LocalRotation, LocalScale)
        : Matrix3X3.Identity;
}
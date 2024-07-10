using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Entities.Systems.Sprite;

public sealed class SpriteComponent : Component
{
    public Color Color = Color.White;
    public Vector2 Position = Vector2.Zero;
    public Vector2 Scale = Vector2.One;
    public bool Visible = true;
}
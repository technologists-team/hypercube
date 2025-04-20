using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Attributes;
using Hypercube.Graphics;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources;

namespace Hypercube.Core.Systems.Rendering;

[RegisterComponent]
public sealed class SpriteComponent : Component
{
    public ResourcePath Path = ResourcePath.Empty;
    public Texture? Texture;
    
    public Vector2 Scale = Vector2.One;
    public Angle Rotation = Angle.Zero;
    public Vector2 Offset = Vector2.Zero;
    public Color Color = Color.White;
}
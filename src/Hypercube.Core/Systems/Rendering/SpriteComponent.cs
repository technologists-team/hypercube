using Hypercube.Core.Graphics.Resources;
using Hypercube.Core.Resources;
using Hypercube.Ecs.Components;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Shapes;

namespace Hypercube.Core.Systems.Rendering;

public struct SpriteComponent() : IComponent
{
    public Texture? Texture;
    
    public ResourcePath Path = ResourcePath.Empty;
    public Vector2 Scale = Vector2.One;
    public Angle Rotation = Angle.Zero;
    public Vector2 Offset = Vector2.Zero;
    public Color Color = Color.White;
    public Rect2 Uv = Rect2.UV;
}

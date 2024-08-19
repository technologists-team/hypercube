using Hypercube.Graphics.Texturing;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Transforms;
using Hypercube.Resources;
using JetBrains.Annotations;

namespace Hypercube.Client.Entities.Systems.Sprite;

[PublicAPI]
public sealed class SpriteComponent : Component
{
    public ITextureHandle TextureHandle = default!;
    public ResourcePath TexturePath;
    public Transform2 Transform = new();
    
    public Color Color = Color.White;
    public bool Visible = true;
}
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Math;
using Hypercube.Math.Transforms;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Entities.Systems.Sprite;

public sealed class SpriteComponent : Component
{
    public ITextureHandle? TextureHandle;
    public ResourcePath TexturePath;
    public Transform2 Transform = new();
    
    public Color Color = Color.White;
    public bool Visible = true;
}
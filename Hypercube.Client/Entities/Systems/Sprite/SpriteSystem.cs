using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Math.Boxs;
using Hypercube.Math.Transforms;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Resources.Caching;

namespace Hypercube.Client.Entities.Systems.Sprite;

public sealed class SpriteSystem : EntitySystem
{
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly IResourceCacher _resourceCacher = default!;
        
    public override void Initialize()
    {
        base.Initialize();
        
        Subscribe<RenderDrawingEvent>(OnRenderDrawing);
        
        Subscribe<SpriteComponent, ComponentAdded>(OnSpriteAdded);
    }

    private void OnSpriteAdded(Entity<SpriteComponent> entity, ref ComponentAdded args)
    {
        entity.Component.TextureHandle = _resourceCacher.GetResource<TextureResource>(entity.Component.TexturePath).Texture;
    }

    private void OnRenderDrawing(ref RenderDrawingEvent args)
    {
        // TODO: Render entities in view space
        foreach (var entity in GetEntities<SpriteComponent>())
        {
            var transform = GetComponent<TransformComponent>(entity);
            Render(entity, transform.Transform);
        }
    }

    public void Render(Entity<SpriteComponent> entity, Transform2 transform)
    {
        _renderer.DrawTexture(entity.Component.TextureHandle, entity.Component.TextureHandle.Texture.QuadCrateTranslated(Vector2.Zero), Box2.UV, entity.Component.Color, transform.Matrix * entity.Component.Transform.Matrix);
    }
}
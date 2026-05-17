using Hypercube.Core.Graphics.Rendering;
using Hypercube.Core.Graphics.Rendering.Context;
using Hypercube.Core.Graphics.Rendering.Manager;
using Hypercube.Core.Graphics.Resources;
using Hypercube.Core.Resources;
using Hypercube.Core.Systems.Transform;
using Hypercube.Ecs;
using Hypercube.Ecs.Lifetime;
using Hypercube.Ecs.Queries;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Systems.Rendering;

public sealed class SpriteSystem : PatchEntitySystem
{
    [Dependency] private readonly IRenderManager _render = null!;
    [Dependency] private readonly IResourceManager _resource = null!;

    private Query _spriteQuery = null!;
    
    public override void Initialize()
    {
        _spriteQuery = CreateQuery(new QueryMeta()
            .WithAll<TransformComponent>()
            .WithAll<SpriteComponent>()
        );
        
        Subscribe<SpriteComponent, AddedEvent>(OnAdded);
    }
    
    private void OnAdded(Entity entity, ref SpriteComponent component, ref AddedEvent args)
    {
        if (component.Texture is not null)
        {
            if (component.Texture.Gpu is null)
                component.Texture.GpuBind(_render.Api);
            
            return;
        }

        component.Texture = _resource.Load<Texture>(component.Path);
        
        if (component.Texture.Gpu is null)
            component.Texture.GpuBind(_render.Api);
    }

    public override void Draw(IRenderContext renderer, DrawPayload payload)
    {
        _spriteQuery.With<TransformComponent, SpriteComponent>((_, ref transform, ref sprite) =>
        {
            if (sprite.Texture is null)
                return;

            if (sprite.Texture.Gpu is null)
                sprite.Texture.GpuBind(_render.Api);

            var position = transform.LocalPosition + sprite.Offset;
            var rotation = transform.LocalRotation.ToEuler().Z + sprite.Rotation;
            var scale = transform.LocalScale * sprite.Scale;
            
            renderer.DrawTexture(sprite.Texture, position.Xy, rotation, scale.Xy, sprite.Color, sprite.Uv);
        });
    }
}
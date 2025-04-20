using Hypercube.Core.Ecs.Attributes;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Core.Systems.Transform;
using Hypercube.Graphics.Rendering.Context;

namespace Hypercube.Core.Systems.Rendering;

[RegisterEntitySystem]
public sealed class SpriteSystem : PatchEntitySystem
{
    private EntityQuery _spriteQuery = default!;
    
    public override void Startup()
    {
        base.Startup();

        _spriteQuery = EntityQueryBuilder
            .With<TransformComponent>()
            .With<SpriteComponent>()
            .Build();
    }

    public override void Draw(IRenderContext renderer)
    {
        var enumerator = _spriteQuery.GetEnumerator;
        while (enumerator.MoveNext(out var entity))
        {
            var transformComponent = GetComponent<TransformComponent>(entity);
            var spriteComponent = GetComponent<SpriteComponent>(entity);

            var position = transformComponent.LocalPosition + spriteComponent.Offset;
            if (spriteComponent.Texture is null)
                continue;
            
            renderer.DrawTexture(spriteComponent.Texture, position, spriteComponent.Color);
        }
    }
}
using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Event;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Math.Transform;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Entities.Systems.Sprite;

public sealed class SpriteSystem : EntitySystem
{
    [Dependency] private readonly IRenderDrawing _drawing = default!;
    [Dependency] private readonly ITextureManager _textureManager = default!;
        
    public override void Initialize()
    {
        base.Initialize();
        
        Subscribe<RenderDrawingEvent>(OnRenderDrawing);
    }

    private void OnRenderDrawing(ref RenderDrawingEvent ev)
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
        _drawing.DrawTexture(entity.Component.TextureHandle ??= _textureManager.GetTextureHandle(entity.Component.TexturePath), Vector2.Zero, entity.Component.Color, transform.Matrix * entity.Component.Transform.Matrix);
    }
}
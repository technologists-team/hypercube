using Hypercube.Client.Entities.Systems.Sprite;
using Hypercube.Math;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Timing;

namespace Hypercube.Example;

public sealed class ExampleSystem : EntitySystem
{
    [Dependency] private readonly ITiming _timing = default!;
    
    public override void FrameUpdate(UpdateFrameEvent args)
    {
        base.FrameUpdate(args);

        foreach (var entity in GetEntities<ExampleComponent>())
        {
            var sprite = GetComponent<SpriteComponent>(entity);
            sprite.Color = Color.FromHSV(MathF.Abs(MathF.Sin((float)_timing.RealTime.TotalMilliseconds / 1000f + entity.Component.Offset)), 1f, 1f);
        }
    }
}
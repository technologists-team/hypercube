using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Math;
using Hypercube.Math.Shapes;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Systems.Physics;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Physics.Shapes;
using SharedPhysicsSystem = Hypercube.Shared.Entities.Systems.Physics.PhysicsSystem;

namespace Hypercube.Client.Entities.Systems.Physics;

public class PhysicsSystem : SharedPhysicsSystem
{
    [Dependency] private readonly IRenderer _renderer = default!;
    
    public override void Initialize()
    {
        base.Initialize();
        
        Subscribe<RenderDrawingEvent>(OnRenderDrawing);
    }

    private void OnRenderDrawing(ref RenderDrawingEvent args)
    {
        foreach (var entity in GetEntities<PhysicsComponent>())
        {
            if (entity.Component.Shape.Type == ShapeType.Circle)
            {
                _renderer.DrawCircle(entity.Component.ShapeCircle, Color.Green);
                continue;
            }
            
            if (entity.Component.Shape.Type == ShapeType.Rectangle)
            {
                _renderer.DrawRectangle(entity.Component.ShapeBox2, Color.Green, true);
                continue;
            }
        }
    }
}
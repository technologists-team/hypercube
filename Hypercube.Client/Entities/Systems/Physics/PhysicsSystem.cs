using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Math;
using Hypercube.Math.Shapes;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Systems.Physics;
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
            var shape = entity.Component.Shape;
            var circle = new Circle(shape.Position + entity.Component.Position, shape.Radius);
            
            _renderer.DrawCircle(circle, Color.Green);
        }
    }
}
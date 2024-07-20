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
                var circle = new Circle(entity.Component.Position + entity.Component.Shape.Position,
                    entity.Component.Shape.Radius);
                _renderer.DrawCircle(circle, Color.Green);
                continue;
            }
            
            if (entity.Component.Shape.Type == ShapeType.Polygon)
            {
                _renderer.DrawPolygon(entity.Component.GetShapeVerticesTransformed(), Color.Green, true);
                continue;
            }
        }
    }
}
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation;
using Hypercube.Shared.Entities.Realisation.Events;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Shared.Entities.Systems.Physics;

public abstract class PhysicsSystem : EntitySystem
{
    [Dependency] private readonly IPhysicsManager _physicsManager = default!;
    [Dependency] private readonly TransformSystem _transformSystem = default!;
    
    public override void Initialize()
    {
        base.Initialize();
        
        Subscribe<PhysicsComponent, ComponentAdded>(OnBodyAdded);
        Subscribe<PhysicsComponent, ComponentRemoved>(OnBodyRemoved);
    }

    public override void FrameUpdate(UpdateFrameEvent args)
    {
        base.FrameUpdate(args);

        foreach (var physics in GetEntities<PhysicsComponent>())
        {
            _transformSystem.SetPosition(physics.Owner, physics.Component.Position);
        }
    }

    private void OnBodyAdded(Entity<PhysicsComponent> entity, ref ComponentAdded args)
    {
        entity.Component.TransformComponent = GetComponent<TransformComponent>(entity);
        entity.Component.TransformSystem = _transformSystem;
        
        _physicsManager.AddBody(entity.Component);
    }

    private void OnBodyRemoved(Entity<PhysicsComponent> entity, ref ComponentRemoved args)
    {
        _physicsManager.RemoveBody(entity.Component);
    }
}
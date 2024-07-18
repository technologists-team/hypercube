using Hypercube.Client.Input;
using Hypercube.Client.Input.Handler;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Entities.Systems.Physics;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Example.Controls;

public sealed class ControlsSystem : EntitySystem
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;

    public override void FrameUpdate(UpdateFrameEvent args)
    {
        base.FrameUpdate(args);

        var inputX = (_inputHandler.IsKeyDown(Key.D) ? 0 : 1) - (_inputHandler.IsKeyDown(Key.A) ? 0 : 1);
        var inputY = (_inputHandler.IsKeyDown(Key.W) ? 0 : 1) - (_inputHandler.IsKeyDown(Key.S) ? 0 : 1);

        foreach (var entity in GetEntities<ControlsComponent>())
        {
            var physics = GetComponent<PhysicsComponent>(entity);
            physics.Velocity = new Vector2(inputX, inputY) * entity.Component.Speed;
        }
    }
}
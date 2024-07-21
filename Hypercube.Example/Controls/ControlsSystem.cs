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

        var inputX = (_inputHandler.IsKeyDown(Key.D) ? 1 : 0) - (_inputHandler.IsKeyDown(Key.A) ? 1 : 0);
        var inputY = (_inputHandler.IsKeyDown(Key.W) ? 1 : 0) - (_inputHandler.IsKeyDown(Key.S) ? 1 : 0);

        foreach (var entity in GetEntities<ControlsComponent>())
        {
            var physics = GetComponent<PhysicsComponent>(entity);
            physics.Force = new Vector2(inputX, inputY) * entity.Component.Speed;
        }
    }
}
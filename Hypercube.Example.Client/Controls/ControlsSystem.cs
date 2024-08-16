using Hypercube.Client.Input.Handler;
using Hypercube.Dependencies;
using Hypercube.Input;
using Hypercube.Mathematics.Vectors;
using Hypercube.Shared.Entities.Realisation.Systems;
using Hypercube.Shared.Entities.Systems.Physics;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Example.Client.Controls;

public sealed class ControlsSystem : EntitySystem
{
    [Dependency] private readonly IInputHandler _inputHandler = default!;

    public override void FrameUpdate(UpdateFrameEvent args)
    {
        base.FrameUpdate(args);

        var inputX = (_inputHandler.IsKeyHeld(Key.D) ? 1 : 0) - (_inputHandler.IsKeyHeld(Key.A) ? 1 : 0);
        var inputY = (_inputHandler.IsKeyHeld(Key.W) ? 1 : 0) - (_inputHandler.IsKeyHeld(Key.S) ? 1 : 0);

        foreach (var entity in GetEntities<ControlsComponent>())
        {
            var physics = GetComponent<PhysicsComponent>(entity);
            physics.Force = new Vector2(inputX, inputY) * entity.Component.Speed;
        }
    }
}
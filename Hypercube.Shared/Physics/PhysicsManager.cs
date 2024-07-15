using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Shared.Physics;

public sealed class PhysicsManager : IPhysicsManager, IEventSubscriber, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly HashSet<World> _worlds = new();
    
    public void PostInject()
    {
        _eventBus.Subscribe<TickFrameEvent>(this, OnTick);
    }

    private void OnTick(ref TickFrameEvent ev)
    {
        UpdateSubSteps(ev.DeltaSeconds, 10);        
    }
    
    private void UpdateSubSteps(float deltaTime, int steps)
    {
        var subDeltaTime = deltaTime / steps;
        for (var i = 0; i < steps; i++)
        {
            Update(subDeltaTime);
        }
    }

    private void Update(float deltaTime)
    {
        foreach (var world in _worlds)
        {
            world.Update(deltaTime);
        }
    }
}
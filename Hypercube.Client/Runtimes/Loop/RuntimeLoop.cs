using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Timing;

namespace Hypercube.Client.Runtimes.Loop;

public sealed class RuntimeLoop : IRuntimeLoop
{
    [Dependency] private readonly ITiming _timing = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    public bool Running { get; private set; }
    
    public void Run()
    {
        Running = true;
        while (Running)
        {
            _timing.StartFrame();

            var deltaTime = (float)_timing.RealFrameTime.TotalSeconds;
            _eventBus.Raise(new InputFrameEvent(deltaTime));
            _eventBus.Raise(new TickFrameEvent(deltaTime));
            _eventBus.Raise(new UpdateFrameEvent(deltaTime));
            _eventBus.Raise(new RenderFrameEvent(deltaTime));
        }
    }

    public void Shutdown()
    {
        Running = false;
    }
}
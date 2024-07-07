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
            _eventBus.Invoke(new InputFrameEvent(deltaTime));
            _eventBus.Invoke(new TickFrameEvent(deltaTime));
            _eventBus.Invoke(new UpdateFrameEvent(deltaTime));
            _eventBus.Invoke(new RenderFrameEvent(deltaTime));
        }
    }

    public void Shutdown()
    {
        Running = false;
    }
}
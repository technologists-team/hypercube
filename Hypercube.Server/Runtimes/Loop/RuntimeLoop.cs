using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Runtime;
using Hypercube.Runtime.Events;
using Hypercube.Shared.Timing;

namespace Hypercube.Server.Runtimes.Loop;

public class RuntimeLoop : IRuntimeLoop
{
    [Dependency] private readonly ITiming _timing = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;

    public bool Running { get; private set; }

    public void Run()
    {
        if (Running)
            throw new InvalidOperationException();

        Running = true;
        while (Running)
        {
            _timing.StartFrame();

            var deltaTime = (float)_timing.RealFrameTime.TotalSeconds;

            _eventBus.Raise(new TickFrameEvent(deltaTime));
            _eventBus.Raise(new UpdateFrameEvent(deltaTime));
        }
    }

    public void Shutdown()
    {
        Running = false;
    }
}
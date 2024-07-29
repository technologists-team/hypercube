using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Loop;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Timing;

namespace Hypercube.Server.Runtimes.Loop;

public class ServerRuntimeLoop : IRuntimeLoop
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
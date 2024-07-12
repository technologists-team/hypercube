using Hypercube.Shared.EventBus.Events.Events;

namespace Hypercube.Shared.Runtimes.Loop.Event;

public readonly struct InputFrameEvent(float deltaSeconds) : IEventArgs
{
    public readonly float DeltaSeconds = deltaSeconds;
}
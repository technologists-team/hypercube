using Hypercube.Shared.EventBus.Events.Events;

namespace Hypercube.Shared.Runtimes.Loop.Event;

public readonly struct UpdateFrameEvent(float deltaSeconds) : IEventArgs
{
    public readonly float DeltaSeconds = deltaSeconds;
}
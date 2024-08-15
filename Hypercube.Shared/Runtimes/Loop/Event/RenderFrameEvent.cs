using Hypercube.EventBus.Events;

namespace Hypercube.Shared.Runtimes.Loop.Event;

public readonly struct RenderFrameEvent(float deltaSeconds) : IEventArgs
{
    public readonly float DeltaSeconds = deltaSeconds;
}
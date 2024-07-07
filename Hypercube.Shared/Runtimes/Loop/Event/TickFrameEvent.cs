namespace Hypercube.Shared.Runtimes.Loop.Event;

public readonly struct TickFrameEvent(float deltaSeconds)
{
    public readonly float DeltaSeconds = deltaSeconds;
}
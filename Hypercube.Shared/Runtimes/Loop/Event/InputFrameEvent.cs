namespace Hypercube.Shared.Runtimes.Loop.Event;

public readonly struct InputFrameEvent(float deltaSeconds)
{
    public readonly float DeltaSeconds = deltaSeconds;
}
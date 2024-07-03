namespace Hypercube.Client.Runtimes.Loop.Event;

public readonly struct UpdateFrameEvent(float deltaSeconds)
{
    public readonly float DeltaSeconds = deltaSeconds;
}
namespace Hypercube.Shared.Runtimes.Loop.Event;

public readonly struct RenderFrameEvent(float deltaSeconds)
{
    public readonly float DeltaSeconds = deltaSeconds;
}
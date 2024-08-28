using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Runtime.Events;

[PublicAPI]
public readonly struct TickFrameEvent : IEventArgs
{
    public readonly float DeltaSeconds;

    public TickFrameEvent(float deltaSeconds)
    {
        DeltaSeconds = deltaSeconds;
    }
}
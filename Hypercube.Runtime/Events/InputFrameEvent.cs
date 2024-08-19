using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Runtime.Events;

[PublicAPI]
public readonly struct InputFrameEvent : IEventArgs
{
    public readonly float DeltaSeconds;

    public InputFrameEvent(float deltaSeconds)
    {
        DeltaSeconds = deltaSeconds;
    }
}
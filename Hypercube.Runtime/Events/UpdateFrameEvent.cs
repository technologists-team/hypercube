using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Runtime.Events;

[PublicAPI]
public readonly struct UpdateFrameEvent : IEventArgs
{
    public readonly float DeltaSeconds;

    public UpdateFrameEvent(float deltaSeconds)
    {
        DeltaSeconds = deltaSeconds;
    }
}
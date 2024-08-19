using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Runtime.Events;

[PublicAPI]
public readonly struct RenderFrameEvent : IEventArgs
{
    public readonly float DeltaSeconds;

    public RenderFrameEvent(float deltaSeconds)
    {
        DeltaSeconds = deltaSeconds;
    }
}
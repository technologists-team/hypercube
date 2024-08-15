using Hypercube.EventBus.Events;
using Hypercube.Math.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Client.Input.Events;

[PublicAPI]
public class ScrollHandledEvent : IEventArgs
{
    public readonly Vector2 Offset;

    public ScrollHandledEvent(Vector2 offset)
    {
        Offset = offset;
    }
}
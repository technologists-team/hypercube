using Hypercube.EventBus.Events;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Client.Input.Events;

public sealed class MousePositionHandledEvent : IEventArgs
{
    public readonly Vector2 Position;

    public MousePositionHandledEvent(Vector2 position)
    {
        Position = position;
    }
}
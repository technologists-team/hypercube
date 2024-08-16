using Hypercube.EventBus.Events;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Client.Input.Events.Windowing;

public class WindowingScrollHandledEvent : IEventArgs
{
    public readonly Vector2 Offset;

    public WindowingScrollHandledEvent(Vector2 offset)
    {
        Offset = offset;
    }
}
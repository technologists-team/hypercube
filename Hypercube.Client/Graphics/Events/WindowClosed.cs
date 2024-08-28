using Hypercube.EventBus.Events;
using Hypercube.Graphics.Windowing;

namespace Hypercube.Client.Graphics.Events;

public readonly struct WindowClosedEvent(WindowHandle handle) : IEventArgs
{
    public readonly WindowHandle Handle = handle;
}
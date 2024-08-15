using Hypercube.Graphics.Windowing;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Graphics.Events;

public readonly struct WindowClosedEvent(WindowHandle handle) : IEventArgs
{
    public readonly WindowHandle Handle = handle;
}
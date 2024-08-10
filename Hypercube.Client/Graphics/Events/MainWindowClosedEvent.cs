using Hypercube.Client.Graphics.Windows;
using Hypercube.Graphics.Windowing;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Graphics.Events;

public readonly struct MainWindowClosedEvent(WindowHandle handle) : IEventArgs
{
    public readonly WindowHandle Handle = handle;
}
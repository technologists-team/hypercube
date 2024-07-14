using Hypercube.Client.Graphics.Windows;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Graphics.Events;

public readonly struct WindowClosedEvent(WindowRegistration registration) : IEventArgs
{
    public readonly WindowRegistration Registration = registration;
}
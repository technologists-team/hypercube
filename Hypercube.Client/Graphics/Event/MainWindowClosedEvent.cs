using Hypercube.Client.Graphics.Windows;

namespace Hypercube.Client.Graphics.Event;

public readonly struct MainWindowClosedEvent(WindowRegistration registration)
{
    public readonly WindowRegistration Registration = registration;
}
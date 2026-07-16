using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowFocus(
    WindowHandle Window,
    bool Focused
) : IEvent;

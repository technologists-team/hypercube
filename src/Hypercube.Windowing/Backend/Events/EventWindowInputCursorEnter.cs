using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowInputCursorEnter(
    WindowHandle Window,
    bool Entered
) : IEvent;

using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowTitle(
    WindowHandle Window,
    string Title
) : IEvent;

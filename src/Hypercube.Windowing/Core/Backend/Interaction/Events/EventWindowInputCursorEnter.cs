using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowInputCursorEnter(
    WindowHandle Window,
    bool Entered
) : IEvent;

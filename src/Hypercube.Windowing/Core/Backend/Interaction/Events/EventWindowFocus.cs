using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowFocus(
    WindowHandle Window,
    bool Focused
) : IEvent;

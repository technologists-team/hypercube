using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowTitle(
    WindowHandle Window,
    string Title
) : IEvent;

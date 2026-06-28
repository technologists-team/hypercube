using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowInputChar(
    WindowHandle Window,
    int CodePoint
) : IEvent;

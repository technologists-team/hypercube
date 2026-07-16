using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowInputChar(
    WindowHandle Window,
    int CodePoint
) : IEvent;

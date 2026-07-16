using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowInputScroll(
    WindowHandle Window,
    Vector2d Offset
) : IEvent;

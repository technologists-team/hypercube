using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowPosition(
    WindowHandle Window,
    Vector2i Position
) : IEvent;

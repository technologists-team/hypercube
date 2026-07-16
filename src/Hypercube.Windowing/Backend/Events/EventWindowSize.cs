using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowSize(
    WindowHandle Window,
    Vector2i Size
) : IEvent;

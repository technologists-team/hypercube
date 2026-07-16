using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Events;

public readonly record struct EventWindowInputCursorPosition(
    WindowHandle Window,
    Vector2d Position
) : IEvent;

using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowInputCursorPosition(
    WindowHandle Window,
    Vector2d Position
) : IEvent;

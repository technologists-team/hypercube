using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowInputScroll(
    WindowHandle Window,
    Vector2d Offset
) : IEvent;

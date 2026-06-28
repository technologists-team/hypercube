using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Events;

public readonly record struct EventWindowSize(
    WindowHandle Window,
    Vector2i Size
) : IEvent;

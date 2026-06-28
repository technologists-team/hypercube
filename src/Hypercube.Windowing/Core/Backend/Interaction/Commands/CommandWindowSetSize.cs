using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Commands;

public readonly record struct CommandWindowSetSize(
    WindowHandle Window,
    Vector2i Size
) : ICommand;

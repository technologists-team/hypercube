using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Commands.Window;

public readonly record struct CommandWindowSetPosition(
    WindowHandle Window,
    Vector2i Position
): ICommand;

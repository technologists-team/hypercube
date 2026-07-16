using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Backend.Commands.Window.Icons;

public readonly record struct CommandWindowSetIcon(
    WindowHandle Window,
    Icon Icon
) : ICommand;

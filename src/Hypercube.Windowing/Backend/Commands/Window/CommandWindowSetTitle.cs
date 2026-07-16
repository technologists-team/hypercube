using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Commands.Window;

public readonly record struct CommandWindowSetTitle(
    WindowHandle Window,
    string Title
) : ICommand;

using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Commands;

public readonly record struct CommandWindowSetTitle(
    WindowHandle Window,
    string Title
) : ICommand;

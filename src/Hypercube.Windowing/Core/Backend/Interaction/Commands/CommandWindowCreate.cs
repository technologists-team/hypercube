using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Core.Backend.Interaction.Commands;

public readonly record struct CommandWindowCreate(
    WindowCreateSettings Settings
) : ICommand<WindowHandle>;

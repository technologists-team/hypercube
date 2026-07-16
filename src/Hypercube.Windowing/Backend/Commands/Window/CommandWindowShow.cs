using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Backend.Commands.Window;

public readonly record struct CommandWindowShow(WindowHandle Window) : ICommand;

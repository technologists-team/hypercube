using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Backend.Commands.Window;

public struct CommandWindowCreateSync(WindowCreateSettings settings) : ISyncCommand<WindowHandle>
{
    public WindowCreateSettings Settings = settings;
    public TaskCompletionSource<WindowHandle>? ResultSource { get; set; }
}

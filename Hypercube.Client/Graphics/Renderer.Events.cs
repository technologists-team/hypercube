using Hypercube.Client.Graphics.Windows;

namespace Hypercube.Client.Graphics;

public sealed partial class Renderer
{
    public event Action<WindowRegistration>? WindowClosed;
    public event Action<WindowRegistration>? MainWindowClosed;
}
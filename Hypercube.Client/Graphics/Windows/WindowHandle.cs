using Hypercube.Client.Graphics.Rendering;

namespace Hypercube.Client.Graphics.Windows;

public class WindowHandle(IRenderer renderer, WindowRegistration registration) : IDisposable
{
    public readonly IRenderer Renderer = renderer;
    public readonly WindowRegistration Registration = registration;

    public void Dispose()
    {
        // Destroy self in renderer
        renderer.DestroyWindow(registration);
    }
}
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Graphics.Windowing;
using JetBrains.Annotations;

namespace Hypercube.Client.Graphics.Windows;

[PublicAPI]
public class WindowRegistration(IRenderer renderer, WindowHandle handle) : IDisposable
{
    public readonly IRenderer Renderer = renderer;
    public readonly WindowHandle Handle = handle;

    public void Dispose()
    {
        // Destroy self in renderer
        Renderer.DestroyWindow(Handle);
    }
}
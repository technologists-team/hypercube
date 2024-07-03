using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Graphics.Windows.Manager.Registrations;

public class GlfwWindowRegistration : WindowRegistration
{
    public unsafe Window* Pointer;
}
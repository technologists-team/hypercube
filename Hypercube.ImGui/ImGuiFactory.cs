using Hypercube.Graphics.Windowing;
using Hypercube.ImGui.Implementations;
using JetBrains.Annotations;

namespace Hypercube.ImGui;

[PublicAPI]
public static class ImGuiFactory
{
    public static IImGuiController Create(WindowHandle window)
    {
        return new OpenGLImGuiController(window);
    }
}
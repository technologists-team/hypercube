using Hypercube.Graphics.Windowing;
using Hypercube.ImGui.Implementations;

namespace Hypercube.ImGui;

public static class ImGuiFactory
{
    public static IImGuiController Create(WindowHandle window)
    {
        return new GlfwImGuiController(window);
    }
}
using Hypercube.Math.Vectors;
using OpenTK.Platform;

namespace Hypercube.ImGui.Implementations;

public partial class GlfwImGuiController
{
    private void UpdateMousePosition(Vector2Int position)
    {
        _io.MousePos = position;
    }
    
    private void UpdateMouseButtons(bool[] state)
    {
        _io.MouseDown[0] = state[0]; // Left
        _io.MouseDown[1] = state[1]; // Right
        _io.MouseDown[2] = state[2]; // Middle
        
        _io.MouseDown[3] = state[3];
        _io.MouseDown[4] = state[4];
    }

    private void UpdateMouseScroll(Vector2 offset)
    {
        _io.MouseWheelH = offset.X;
        _io.MouseWheel = offset.Y;
    }
    
    private void UpdateMouseCursor()
    {

    }
}
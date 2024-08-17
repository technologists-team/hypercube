using Hypercube.Input;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.ImGui.Implementations;

public partial class GlfwImGuiController
{
    private const int MouseButtons = 5; 
    
    public void InputFrame()
    {
        // Clear mouse input
        for (var i = 0; i < MouseButtons; i++)
        {
            _io.MouseDown[i] = false;
        }
    }
    
    public void UpdateMousePosition(Vector2Int position)
    {
        _io.MousePos = position;
    }
    
    public void UpdateMouseButtons(MouseButton button, bool state)
    {
        var index = (int) button;
        if (index >= MouseButtons)
            return;
        
        _io.MouseDown[index] = state;
    }

    public void UpdateMouseScroll(Vector2 offset)
    {
        _io.MouseWheelH = offset.X;
        _io.MouseWheel = offset.Y;
    }
    
    public void UpdateMouseCursor()
    {

    }
}
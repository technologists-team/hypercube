using Hypercube.Mathematics.Vectors;

namespace Hypercube.Windowing.Core.Windows;

public sealed partial class Window
{
    private void CloseCallback(WindowHandle window)
    {
        if (!WindowFilter(window))
            return;
        
        OnClose?.Invoke();
    }

    private void SizeCallback(WindowHandle window, Vector2i size)
    {
        if (!WindowFilter(window))
            return;
        
        Size = size;
        OnSize?.Invoke(size);
    }

    private void FramebufferSizeCallback(WindowHandle window, Vector2i size)
    {
        if (!WindowFilter(window))
            return;
        
        FramebufferSize = size;
    }

    private void PositionCallback(WindowHandle window, Vector2i position)
    {
        if (!WindowFilter(window))
            return;
        
        Position = position;
    }

    private void FocusCallback(WindowHandle window, bool focused)
    {
        if (!WindowFilter(window))
            return;
        
        Focus = focused;
    }
}

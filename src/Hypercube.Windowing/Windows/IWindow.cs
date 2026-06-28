using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Windows;

public interface IWindow
{
    event RouterWindowCloseHandler? OnClose;
    
    Vector2i Size { get; }
    Vector2i FramebufferSize { get; }
    Vector2i Position { get; }
    bool Focus { get; }

    void SetIcon(Icon icon);
    void SetPosition(Vector2i position);
    void SetSize(Vector2i size);
    void SetTitle(string title);

    void Destroy();
}

using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Backend.Interaction.Handlers;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Backend;

public interface IBackend
{
    event ErrorHandler? OnError;
    
    event MonitorStateHandler? OnMonitorState;
    event JoystickStateHandler? OnJoystickState;
    
    event WindowCloseHandler? OnWindowClose;
    event WindowSizeHandler? OnWindowSize;
    event WindowFramebufferSizeHandler? OnWindowFramebufferSize;
    event WindowPositionHandler? OnWindowPosition;
    event WindowFocusHandler? OnWindowFocus;
    
    event WindowInputKeyHandler? OnWindowInputKey;
    event WindowInputCursorHandler? OnWindowInputCursor;
    event WindowInputMouseButtonHandler? OnWindowInputMouseButton;
    event WindowInputScrollHandler? OnWindowInputScroll;
    
    bool Initialize();
    
    void Terminate();

    WindowHandle WindowCreate(WindowCreateSettings settings);

    void WindowDestroy(WindowHandle window);

    void WindowFocus(WindowHandle window);

    void WindowAttention(WindowHandle window);

    void WindowSetPosition(WindowHandle window, Vector2i position);

    void WindowSetSize(WindowHandle window, Vector2i size);

    void WindowSetTitle(WindowHandle window, string title);

    void WindowSetIcon(WindowHandle window, Icon icon);

    void WindowSetIcon(WindowHandle window, ReadOnlySpan<Icon> icons);

    void WindowSetIcon(WindowHandle window, Icon[] icons);

    void MakeContextCurrent(WindowHandle window);

    void SwapBuffers(WindowHandle window);
    
    nint GetProcAddress(string procName);

    void PollEvents();

    void WaitEvents();

    void PostEmptyEvent();
}

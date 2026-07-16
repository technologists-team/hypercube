using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Backend.Handlers;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

namespace Hypercube.Windowing.Backend;

// NOTE: change to void ExecuteCommands(IUnsafeCommandBuffer commandBuffer); ???
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
    
    void PollEvents();

    void WaitEvents();

    void PostEmptyEvent();

    WindowHandle WindowCreate(WindowCreateSettings settings);

    void WindowDestroy(WindowHandle window);

    void WindowFocus(WindowHandle window);

    void WindowAttention(WindowHandle window);
    
    void WindowShow(WindowHandle window);
    
    void WindowHide(WindowHandle window);
    
    void WindowMinimize(WindowHandle window);
    
    void WindowMaximize(WindowHandle window);
    
    void WindowRestore(WindowHandle window);

    void WindowSetContext(WindowHandle window);
    
    void WindowSetPosition(WindowHandle window, Vector2i position);

    void WindowSetSize(WindowHandle window, Vector2i size);

    void WindowSetTitle(WindowHandle window, string title);

    void WindowSetIcon(WindowHandle window, Icon icon);

    void WindowSetIcons(WindowHandle window, ReadOnlySpan<Icon> icons);

    void WindowSetIcons(WindowHandle window, Icon[] icons);

    WindowHandle WindowGetContext();

    void WindowSwapBuffers(WindowHandle window);

    nint GetProcAddress(string procName);
}

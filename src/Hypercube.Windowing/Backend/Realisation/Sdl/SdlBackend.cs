using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Windows;

// Silk redefine for reduce type/namespace collisions
using SilkSdl = Silk.NET.SDL.Sdl;
using SilkWindow = Silk.NET.SDL.Window;
using SilkWindowFlags = Silk.NET.SDL.WindowFlags;
using SilkGLattr = Silk.NET.SDL.GLattr;
using SilkGLprofile = Silk.NET.SDL.GLprofile;
using SilkEventType = Silk.NET.SDL.EventType;
using SilkWindowEventID = Silk.NET.SDL.WindowEventID;
using SilkEvent = Silk.NET.SDL.Event;

namespace Hypercube.Windowing.Backend.Realisation.Sdl;
/*
public unsafe class SdlBackend : IBackend
{
    public const int SilkTrue = 1;
    
    public event ErrorHandler? OnError;
    public event MonitorStateHandler? OnMonitorState;
    public event JoystickStateHandler? OnJoystickState;
    
    public event WindowCloseHandler? OnWindowClose;
    public event WindowSizeHandler? OnWindowSize;
    public event WindowFramebufferSizeHandler? OnWindowFramebufferSize;
    public event WindowPositionHandler? OnWindowPosition;
    public event WindowFocusHandler? OnWindowFocus;
    
    public event WindowInputKeyHandler? OnWindowInputKey;
    public event WindowInputCursorHandler? OnWindowInputCursor;
    public event WindowInputMouseButtonHandler? OnWindowInputMouseButton;
    public event WindowInputScrollHandler? OnWindowInputScroll;

    private readonly Dictionary<nint, nint> _glContexts = new();
    
    private readonly SilkSdl _sdl = SilkSdl.GetApi();

    public bool Initialize()
    {
        return _sdl.Init(SilkSdl.InitVideo | SilkSdl.InitEvents | SilkSdl.InitJoystick | SilkSdl.InitGamecontroller) == 0;
    }

    public void Terminate()
    {
        _sdl.Quit();
    }

    public void PollEvents()
    {
        SilkEvent e;
        
        while (_sdl.PollEvent(&e) == SilkTrue)
        {
            ProcessEvent(e);
        }
    }

    public void WaitEvents()
    {
        SilkEvent e;
        
        if (_sdl.WaitEvent(&e) != SilkTrue)
            return;
        
        ProcessEvent(e);
        
        while (_sdl.PollEvent(&e) == SilkTrue)
        {
            ProcessEvent(e);
        }
    }

    public void PostEmptyEvent()
    {
        SilkEvent e = default;
        e.Type = (uint)SilkEventType.Userevent;
        _sdl.PushEvent(&e);
    }

    public WindowHandle WindowCreate(WindowCreateSettings settings)
    {
        var width = settings.Size.X;
        var height = settings.Size.Y;
        var title = settings.Title;
        
        var version = settings.ContextVersion;
        var context = settings.ContextType;
        
        uint flags = (uint)(SilkWindowFlags.Resizable | SilkWindowFlags.AllowHighdpi);
        
        switch (context)
        {
            case WindowContextType.InternalDesktop:
                _sdl.GLSetAttribute(SilkGLattr.ContextMajorVersion, version.Major);
                _sdl.GLSetAttribute(SilkGLattr.ContextMinorVersion, version.Minor);
                _sdl.GLSetAttribute(SilkGLattr.ContextProfileMask, (int)SilkGLprofile.Core);
                flags |= (uint)SilkWindowFlags.Opengl;
                break;
                
            case WindowContextType.InternalEmbedded:
                _sdl.GLSetAttribute(SilkGLattr.ContextProfileMask, (int)SilkGLprofile.ES);
                _sdl.GLSetAttribute(SilkGLattr.ContextMajorVersion, version.Major);
                _sdl.GLSetAttribute(SilkGLattr.ContextMinorVersion, version.Minor);
                flags |= (uint)SilkWindowFlags.Opengl;
                break;
                
            case WindowContextType.External:
                break;
        }
        
        const int SDL_WINDOWPOS_CENTERED = 0x2FFF0000;
        
        // CreateWindow возвращает SilkWindow* (Silk.NET.SDL.Window*)
        var window = _sdl.CreateWindow(title, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, width, height, flags);
        
        if (window == null)
            return WindowHandle.Null;
            
        var handleAddress = Translate(window);
        
        if (context != WindowContextType.External)
        {
            var glContext = _sdl.GLCreateContext(window);
            _glContexts[handleAddress.Value] = glContext;
        }
        
        WindowSetIcon(handleAddress, settings.Icon);
        
        return handleAddress;
    } 

    public void WindowDestroy(WindowHandle window)
    {
        if (_glContexts.TryGetValue(window.Value, out var glContext))
        {
            _sdl.GLDeleteContext(glContext);
            _glContexts.Remove(window.Value);
        }
        
        _sdl.DestroyWindow((SilkWindow*)window.Value);
    }

    public void WindowFocus(WindowHandle window)
    {
        _sdl.RaiseWindow((SilkWindow*)window.Value);
    }

    public void WindowAttention(WindowHandle window)
    {
        // В SDL 2.0.16+ есть FlashWindow, но для совместимости используем RaiseWindow
        _sdl.RaiseWindow((SilkWindow*)window.Value);
    }

    public void WindowShow(WindowHandle window)
    {
        _sdl.ShowWindow((SilkWindow*)window.Value);
    }

    public void WindowHide(WindowHandle window)
    {
        _sdl.HideWindow((SilkWindow*)window.Value);
    }

    public void WindowMinimize(WindowHandle window)
    {
        _sdl.MinimizeWindow((SilkWindow*)window.Value);
    }

    public void WindowMaximize(WindowHandle window)
    {
        _sdl.MaximizeWindow((SilkWindow*)window.Value);
    }

    public void WindowRestore(WindowHandle window)
    {
        _sdl.RestoreWindow((SilkWindow*)window.Value);
    }

    public void WindowSetPosition(WindowHandle window, Vector2i position)
    {
        _sdl.SetWindowPosition((SilkWindow*)window.Value, position.X, position.Y);
    }

    public void WindowSetSize(WindowHandle window, Vector2i size)
    {
        _sdl.SetWindowSize((SilkWindow*)window.Value, size.X, size.Y);
    }

    public void WindowSetTitle(WindowHandle window, string title)
    {
        _sdl.SetWindowTitle((SilkWindow*)window.Value, title);
    }

    public void WindowSetIcon(WindowHandle window, Icon icon)
    {
        fixed (byte* pixels = icon.Data)
        {
            var width = icon.Size.X;
            var height = icon.Size.Y;
            
            // Маски для Little Endian RGBA.
            uint rmask = 0x000000FF;
            uint gmask = 0x0000FF00;
            uint bmask = 0x00FF0000;
            uint amask = 0xFF000000;
            
            var surface = _sdl.CreateRGBSurfaceFrom(
                pixels, 
                width, 
                height, 
                32, 
                width * 4, 
                rmask, gmask, bmask, amask
            );
            
            if (surface != null)
            {
                _sdl.SetWindowIcon((SilkWindow*)window.Value, surface);
                _sdl.FreeSurface(surface);
            }
        }
    }
    
    public void WindowSetIcons(WindowHandle window, ReadOnlySpan<Icon> icons)
    {
        if (icons.IsEmpty)
            return;
            
        // SDL поддерживает только одну иконку на окно. Выбираем самую большую.
        var bestIcon = icons[0];
        foreach (ref readonly var icon in icons)
        {
            if (icon.Size.X * icon.Size.Y > bestIcon.Size.X * bestIcon.Size.Y)
            {
                bestIcon = icon;
            }
        }
        
        WindowSetIcon(window, bestIcon);
    }

    public void WindowSetIcons(WindowHandle window, Icon[] icons)
    {
        WindowSetIcons(window, new ReadOnlySpan<Icon>(icons));
    }

    public void WindowSetContext(WindowHandle window)
    {
        if (_glContexts.TryGetValue(window.Value, out var glContext))
        {
            _sdl.GLMakeCurrent((SilkWindow*)window.Value, glContext);
        }
    }

    public nint GetProcAddress(string procName)
    {
        return (nint)_sdl.GLGetProcAddress(procName);
    }

    public void WindowSwapBuffers(WindowHandle window)
    {
        _sdl.GLSwapWindow((SilkWindow*)window.Value);
    }

    public WindowHandle WindowGetContext()
    {
        var window = _sdl.GLGetCurrentWindow();
        return Translate(window);
    }
    
    // Утилиты для преобразования указателей и ID
    private WindowHandle Translate(SilkWindow* window) => new WindowHandle((nint)window);
    private WindowHandle Translate(uint windowId) => Translate(_sdl.GetWindowFromID(windowId));
    
    // Обработчик событий SDL
    private void ProcessEvent(SilkEvent e)
    {
        switch ((SilkEventType)e.Type)
        {
            case SilkEventType.Quit:
                // SDL_QUIT не привязан к конкретному окну. 
                break;

            case SilkEventType.Windowevent:
            {
                var windowHandle = Translate(e.Window.WindowID);
                switch ((SilkWindowEventID)e.Window.Event)
                {
                    case SilkWindowEventID.Close:
                        OnWindowClose?.Invoke(windowHandle);
                        break;
                    case SilkWindowEventID.Resized:
                    case SilkWindowEventID.SizeChanged:
                        OnWindowSize?.Invoke(windowHandle, new Vector2i(e.Window.Data1, e.Window.Data2));
                        break;
                    case SilkWindowEventID.Moved:
                        OnWindowPosition?.Invoke(windowHandle, new Vector2i(e.Window.Data1, e.Window.Data2));
                        break;
                    case SilkWindowEventID.FocusGained:
                        OnWindowFocus?.Invoke(windowHandle, true);
                        break;
                    case SilkWindowEventID.FocusLost:
                        OnWindowFocus?.Invoke(windowHandle, false);
                        break;
                }
                break;
            }

            case SilkEventType.Keydown:
            case SilkEventType.Keyup:
            {
                var windowHandle = Translate(e.Key.WindowID);
                // TODO: Замените на ваш маппинг SDL сканкодов в Hypercube Key enum
                // var action = e.Type == (uint)SilkEventType.Keydown ? InputAction.Press : InputAction.Release;
                // OnWindowInputKey?.Invoke(windowHandle, MapKey(e.Key.Keysym.Scancode), (int)e.Key.Keysym.Scancode, action, MapMods(e.Key.Keysym.Mod));
                break;
            }

            case SilkEventType.Mousemotion:
            {
                var windowHandle = Translate(e.Motion.WindowID);
                OnWindowInputCursor?.Invoke(windowHandle, new Vector2d(e.Motion.X, e.Motion.Y));
                break;
            }

            case SilkEventType.Mousebuttondown:
            case SilkEventType.Mousebuttonup:
            {
                var windowHandle = Translate(e.Button.WindowID);
                // TODO: Замените на ваш маппинг SDL кнопок в Hypercube MouseButton enum
                // var action = e.Type == (uint)SilkEventType.Mousebuttondown ? InputAction.Press : InputAction.Release;
                // OnWindowInputMouseButton?.Invoke(windowHandle, MapButton(e.Button.Button), action, 0);
                break;
            }

            case SilkEventType.Mousewheel:
            {
                var windowHandle = Translate(e.Wheel.WindowID);
                OnWindowInputScroll?.Invoke(windowHandle, new Vector2d(e.Wheel.X, e.Wheel.Y));
                break;
            }
        }
    }
}
*/
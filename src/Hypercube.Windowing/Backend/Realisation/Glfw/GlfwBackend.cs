using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using Hypercube.Windowing.Core.Backend.Interaction.Handlers;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Types;
using Hypercube.Windowing.Windows;

// Silk redefine for reduce type/namespace collisions
using SilkGlfw = Silk.NET.GLFW.Glfw;
using SilkMonitor = Silk.NET.GLFW.Monitor;
using SilkWindow = Silk.NET.GLFW.WindowHandle;
using SilkImage = Silk.NET.GLFW.Image;
using SilkWindowClientApi= Silk.NET.GLFW.ClientApi;
using SilkWindowOpenGlProfile = Silk.NET.GLFW.OpenGlProfile;
using SilkWindowHintOpenGlProfiler = Silk.NET.GLFW.WindowHintOpenGlProfile;
using SilkWindowHintClientApi = Silk.NET.GLFW.WindowHintClientApi;
using SilkWindowHintInt = Silk.NET.GLFW.WindowHintInt;

namespace Hypercube.Windowing.Backend.Realisation.Glfw;

public sealed unsafe partial class GlfwBackend : IBackend
{
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
    
    private readonly SilkGlfw _glfw = SilkGlfw.GetApi();

    public bool Initialize()
    {
        _glfw.SetErrorCallback(ErrorCallback);
        
        if (!_glfw.Init())
            return false;

        _glfw.SetMonitorCallback(MonitorCallback);
        _glfw.SetJoystickCallback(JoystickCallback);
        return true;
    }

    public void Terminate()
    {
        _glfw.Terminate();
    }

    public WindowHandle WindowCreate(WindowCreateSettings settings)
    {
        var width = settings.Size.X;
        var height = settings.Size.Y;
        var title = settings.Title;
        
        var monitor = (SilkMonitor*) null;
        var window = (SilkWindow*) null;

        var version = settings.ContextVersion;
        var context = settings.ContextType;
        
        switch (context)
        {
            case WindowContextType.InternalDesktop:
                _glfw.WindowHint(SilkWindowHintClientApi.ClientApi, SilkWindowClientApi.OpenGL);
                _glfw.WindowHint(SilkWindowHintInt.ContextVersionMajor, version.Major);
                _glfw.WindowHint(SilkWindowHintInt.ContextVersionMinor, version.Minor);
                _glfw.WindowHint(SilkWindowHintOpenGlProfiler.OpenGlProfile, SilkWindowOpenGlProfile.Core);
                break;
            
            case WindowContextType.InternalEmbedded:
                _glfw.WindowHint(SilkWindowHintClientApi.ClientApi, SilkWindowClientApi.OpenGLES); 
                _glfw.WindowHint(SilkWindowHintInt.ContextVersionMajor, version.Major);
                _glfw.WindowHint(SilkWindowHintInt.ContextVersionMinor, version.Minor);
                break;
            
            case WindowContextType.External:
                _glfw.WindowHint(SilkWindowHintClientApi.ClientApi, SilkWindowClientApi.NoApi);
                break;
        }
        
        var handle = _glfw.CreateWindow(width, height, title, monitor, window);
        if (handle is null)
        {
            return WindowHandle.Null;
        }
        
        // Setters
        WindowSetIcon(handle, settings.Icon);
        
        // Callbacks input
        _glfw.SetKeyCallback(handle, WindowKeyCallback);
        _glfw.SetScrollCallback(handle, WindowScrollCallback);
        _glfw.SetMouseButtonCallback(handle, WindowMouseButtonCallback);
        _glfw.SetCursorPosCallback(handle, WindowCursorCallback);
        
        // Callbacks state
        _glfw.SetWindowCloseCallback(handle, WindowCloseCallback);
        _glfw.SetWindowSizeCallback(handle, WindowSizeCallback);
        _glfw.SetFramebufferSizeCallback(handle, WindowFramebufferSizeCallback);
        _glfw.SetWindowPosCallback(handle, WindowPositionCallback);
        _glfw.SetWindowFocusCallback(handle, WindowFocusCallback);
        
        return Translate(handle);
    } 

    public void WindowDestroy(WindowHandle window)
    {
        WindowDestroy((SilkWindow*) window.Value);
    }

    public void WindowFocus(WindowHandle window)
    {
        WindowFocus((SilkWindow*) window.Value);
    }

    public void WindowAttention(WindowHandle window)
    {
        WindowAttention((SilkWindow*) window.Value);
    }

    public void WindowSetPosition(WindowHandle window, Vector2i position)
    {
        WindowSetPosition((SilkWindow*) window.Value, position);
    }

    public void WindowSetSize(WindowHandle window, Vector2i size)
    {
        WindowSetSize((SilkWindow*) window.Value, size);
    }

    public void WindowSetTitle(WindowHandle window, string title)
    {
        WindowSetTitle((SilkWindow*) window.Value, title);
    }

    public void WindowSetIcon(WindowHandle window, Icon icon)
    {
        WindowSetIcon((SilkWindow*) window.Value, icon);
    }
    
    public void WindowSetIcon(WindowHandle window, ReadOnlySpan<Icon> icons)
    {
        WindowSetIcons((SilkWindow*) window.Value, icons);
    }
    
    public void WindowSetIcon(WindowHandle window, Icon[] icons)
    {
        WindowSetIcons((SilkWindow*) window.Value, icons.AsSpan());
    }

    public void MakeContextCurrent(WindowHandle window)
    {
        _glfw.MakeContextCurrent((SilkWindow*) window.Value);
    }

    public nint GetProcAddress(string procName)
    {
        return _glfw.GetProcAddress(procName);
    }

    public void SwapBuffers(WindowHandle window)
    {
        _glfw.SwapBuffers((SilkWindow*) window.Value);
    }

    public void PollEvents()
    {
        _glfw.PollEvents();
    }

    public void WaitEvents()
    {
        _glfw.WaitEvents();
    }

    public void PostEmptyEvent()
    {
        _glfw.PostEmptyEvent();
    }
    
    private void WindowDestroy(SilkWindow* window)
    {
        _glfw.DestroyWindow(window);
    }

    private void WindowFocus(SilkWindow* window)
    {
        _glfw.FocusWindow(window);
    }

    private void WindowAttention(SilkWindow* window)
    {
        _glfw.RequestWindowAttention(window);
    }
    
    private void WindowSetPosition(SilkWindow* window, Vector2i position)
    {
        _glfw.SetWindowPos(window, position.X, position.Y);
    }

    private void WindowSetSize(SilkWindow* window, Vector2i size)
    {
        _glfw.SetWindowSize(window, size.X, size.Y);
    }

    private void WindowSetTitle(SilkWindow* window, string title)
    {
        _glfw.SetWindowTitle(window, title);
    }
    
    private void WindowSetIcon(SilkWindow* window, Icon icon)
    {
        fixed (byte* pixels = icon.Data)
        {
            var width = icon.Size.X;
            var height = icon.Size.Y;
            
            var silkImage = new SilkImage
            {
                Width = width,
                Height = height,
                Pixels = pixels
            };

            _glfw.SetWindowIcon(window, 1, &silkImage);
        }
    }
    
    private void WindowSetIcons(SilkWindow* window, ReadOnlySpan<Icon> icons)
    {
        if (icons.IsEmpty)
            return;
        
        var totalBytes = 0;
        foreach (ref readonly var icon in icons)
        {
            totalBytes += icon.Data.Length;
        }
        
        Span<byte> data = stackalloc byte[totalBytes];
        var pointer = (byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(data));
    
        var images = new SilkImage[icons.Length];
        var offset = 0;
    
        for (var i = 0; i < icons.Length; i++)
        {
            ref readonly var icon = ref icons[i];
            var length = icon.Data.Length;
            
            icon.Data.CopyTo(data.Slice(offset, length));

            images[i] = new SilkImage
            {
                Width = icon.Size.X,
                Height = icon.Size.Y,
                Pixels = pointer + offset
            };
        
            offset += length;
        }
        
        fixed (SilkImage* ptr = images)
            _glfw.SetWindowIcon(window, images.Length, ptr);
    }
}

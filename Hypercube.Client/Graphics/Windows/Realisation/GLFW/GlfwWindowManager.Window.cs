using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Hypercube.Graphics;
using Hypercube.Graphics.Texturing;
using Hypercube.Graphics.Windowing;
using Hypercube.Mathematics.Vectors;
using Hypercube.OpenGL;
using Hypercube.OpenGL.Utilities.Helpers;
using Hypercube.Resources;
using Hypercube.Resources.Manager;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwImage = OpenTK.Windowing.GraphicsLibraryFramework.Image;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;
using MonitorHandle = Hypercube.Graphics.Monitors.MonitorHandle;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

public sealed unsafe partial class GlfwWindowing
{
    public WindowCreateResult WindowCreate(IContextInfo? context, WindowCreateSettings settings,
        WindowHandle? contextShare)
    {
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintString.X11ClassName, "Hypercube");
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintString.X11InstanceName, "Hypercube");

        if (context is not ContextInfo info)
        {
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
        }
        else
        {
#if DEBUG
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, true);
#endif
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.ContextVersionMajor, info.Version.Major);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.ContextVersionMinor, info.Version.Minor);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, info.Compatibility);
            
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.SrgbCapable, true);

            switch (info.Profile)
            {
                case ContextProfile.Compatability:
                    OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Any);
                    OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                    break;

                case ContextProfile.Core:
                    OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
                    OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                    break;

                case ContextProfile.Any:
                    OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Any);
                    OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlEsApi);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintContextApi.ContextCreationApi,
                info.Api == ContextApi.EglContextApi
                    ? ContextApi.EglContextApi
                    : ContextApi.NativeContextApi);
        }

        Window* share = null;
        if (contextShare is GlfwWindowHandle glfwShare)
            share = glfwShare;

        Monitor* monitor = null;
        if (settings.Monitor is null || !_monitors.TryGetValue(settings.Monitor.Id, out var threadMonitorRegistration))
        {

            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.RedBits, settings.RedBits ?? 8);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.GreenBits, settings.GreenBits ?? 8);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.BlueBits, settings.BlueBits ?? 8);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.RefreshRate, -1);
        }
        else
        {
            monitor = threadMonitorRegistration.Pointer;
            var modePointer = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.GetVideoMode(monitor);

            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.RedBits, settings.RedBits ?? modePointer->RedBits);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.GreenBits, settings.GreenBits ?? modePointer->GreenBits);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.BlueBits, settings.BlueBits ?? modePointer->BlueBits);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.BlueBits, modePointer->RefreshRate);
        }

        if (settings.AlphaBits is not null)
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.AlphaBits, settings.AlphaBits.Value);

        if (settings.DepthBits is not null)
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.DepthBits, settings.DepthBits.Value);

        if (settings.StencilBits is not null)
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.StencilBits, settings.StencilBits.Value);

        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.Resizable, settings.Resizable);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.TransparentFramebuffer, settings.TransparentFramebuffer);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.Decorated, settings.Decorated);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.Visible, settings.Visible);

        var window = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.CreateWindow(
            settings.Width,
            settings.Height,
            settings.Title,
            monitor,
            share);

        if (window is null)
        {
            Terminate();
            return new WindowCreateResult(GLFWHelper.GetError());
        }

        return new WindowCreateResult(WindowSetup(window, settings));
    }

    private WindowHandle WindowSetup(Window* window, WindowCreateSettings settings)
    {
        GLFWHelper.GetFramebufferSize(window, out var framebufferSize);
        GLFWHelper.GetWindowSize(window, out var size);

        var handle = new GlfwWindowHandle(new WindowId(_nextWindowId++), window)
        {
            Ratio = framebufferSize.AspectRatio,
            Size = size,
            FramebufferSize = framebufferSize
        };

        // Setting icons
        if (settings.WindowImages != null)
            WindowSetIcons(handle, settings.WindowImages.ToList());

        // Setting callbacks
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowCloseCallback(window, _windowCloseCallback);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowSizeCallback(window, _windowSizeCallback);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowFocusCallback(window, _windowFocusCallback);

        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetCharCallback(window, _charCallback);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetScrollCallback(window, _scrollCallback);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetKeyCallback(window, _keyCallback);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetMouseButtonCallback(window, _mouseButtonCallback);
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetCursorPosCallback(window, _cursorPosCallback);

        return handle;
    }
     
     public void WindowDestroy(WindowHandle handle)
     {
         if (handle is not GlfwWindowHandle glfwRegistration)
             return;
         
         if (OperatingSystem.IsWindows() && glfwRegistration.Owner is not null)
         {
             // On Windows, closing the child window causes the owner to be minimized, apparently.
             // Clear owner on close to avoid this.

             /*
             var hWnd = (HWND) GLFW.GetWin32Window(window);
             DebugTools.Assert(hWnd != HWND.NULL);

             Windows.SetWindowLongPtrW(
                 hWnd,
                 GWLP.GWLP_HWNDPARENT,
                 0);
             */
         }

         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.DestroyWindow(glfwRegistration);
     }

     private bool TryGetWindow(Window* window, [NotNullWhen(true)] out GlfwWindowHandle? registration)
     {
         registration = GetWindow(window);
         return registration is not null;
     }

     private GlfwWindowHandle? GetWindow(Window* window)
     {
         foreach (var (_, registration) in _renderer.Windows)
         {
             if (registration is not GlfwWindowHandle glfwRegistration)
                 continue;
             
             if (glfwRegistration != window)
                 continue;

             return glfwRegistration;
         }

         return null;
     }

     public void WindowSetTitle(WindowHandle window, string title)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;
        
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowTitle(glfwWindow, title);
     }
     
     public void WindowSetMonitor(WindowHandle window, MonitorHandle registration)
     {
         WindowSetMonitor(window, registration, Vector2i.Zero);
     }
     
     public void WindowRequestAttention(WindowHandle window)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;
        
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.RequestWindowAttention(glfwWindow);
     }

     public void WindowSetSize(WindowHandle window, Vector2i size)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;
        
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowSize(glfwWindow, size.X, size.Y);
     }

     public void WindowSetVisible(WindowHandle window, bool visible)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;

         if (visible)
         {
             OpenTK.Windowing.GraphicsLibraryFramework.GLFW.ShowWindow(glfwWindow);
             return;
         }
            
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.HideWindow(glfwWindow);
     }

     public void WindowSetOpacity(WindowHandle window, float opacity)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;
        
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowOpacity(glfwWindow, opacity);
     }
     
     public void WindowSetPosition(WindowHandle window, Vector2i position)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;
         
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowPos(glfwWindow, position.X, position.Y);
     }

     public void WindowSwapBuffers(WindowHandle window)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;
        
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SwapBuffers(glfwWindow);
     }
     
     public void WindowSetIcons(WindowHandle window, List<ITexture> images)
     {
         if (window is not GlfwWindowHandle glfwWindow)
             return;

         var count = images.Count;
         
         // ReSharper disable once SuggestVarOrType_Elsewhere
         Span<GCHandle> handles = stackalloc GCHandle[count];
         Span<GlfwImage> glfwImages = stackalloc GlfwImage[count];
         
         for (var i = 0; i < count; i++)
         {
             var image = images[i];
             handles[i] = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
             var addrOfPinnedObject = (byte*) handles[i].AddrOfPinnedObject();
             glfwImages[i] = new GlfwImage(image.Width, image.Height, addrOfPinnedObject);
         }
         
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowIcon(glfwWindow, glfwImages);
     }
     
     public IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureMan, IResourceLoader resourceLoader, ResourcePath path)
     {
         var files = resourceLoader.FindContentFiles(path);
         
         foreach (var file in files)
         {
             yield return textureMan.CreateTexture(file);
         }
     }
     
     private class GlfwWindowHandle : WindowHandle
     {
         public readonly Window* GlfwPointer;

         public GlfwWindowHandle(WindowId id, Window* pointer) : base(id, (nint)pointer)
         {
             GlfwPointer = pointer;
         }

         public static implicit operator Window*(GlfwWindowHandle handle)
         {
             return handle.GlfwPointer;
         }
     }
}
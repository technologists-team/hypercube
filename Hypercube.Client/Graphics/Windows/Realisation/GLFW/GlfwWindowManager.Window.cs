using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Realisation.OpenGL;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Utilities.Helpers;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwImage = OpenTK.Windowing.GraphicsLibraryFramework.Image;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace Hypercube.Client.Graphics.Windows.Realisation.Glfw;

public sealed unsafe partial class GlfwWindowManager
{
    public WindowCreateResult WindowCreate(ContextInfo? context, WindowCreateSettings settings,
        WindowRegistration? contextShare)
    {
        GLFW.WindowHint(WindowHintString.X11ClassName, "Hypercube");
        GLFW.WindowHint(WindowHintString.X11InstanceName, "Hypercube");

        if (context is null)
        {
            GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
        }
        else
        {
#if DEBUG
            GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, true);
#endif
            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, context.Value.Version.Major);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, context.Value.Version.Minor);
            GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, context.Value.Compatibility);
            GLFW.WindowHint(WindowHintBool.SrgbCapable, true);

            switch (context.Value.Profile)
            {
                case ContextProfile.Compatability:
                    GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Any);
                    GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                    break;

                case ContextProfile.Core:
                    GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
                    GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                    break;

                case ContextProfile.Any:
                    GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Any);
                    GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlEsApi);
                    break;
            }

            GLFW.WindowHint(WindowHintContextApi.ContextCreationApi,
                context.Value.Api == ContextApi.EglContextApi
                    ? ContextApi.EglContextApi
                    : ContextApi.NativeContextApi);
        }

        Window* share = null;
        if (contextShare is GlfwWindowRegistration glfwShare)
            share = glfwShare.Pointer;

        Monitor* monitor = null;
        if (settings.Monitor is null || !_monitors.TryGetValue(settings.Monitor.Id, out var threadMonitorRegistration))
        {

            GLFW.WindowHint(WindowHintInt.RedBits, settings.RedBits ?? 8);
            GLFW.WindowHint(WindowHintInt.GreenBits, settings.GreenBits ?? 8);
            GLFW.WindowHint(WindowHintInt.BlueBits, settings.BlueBits ?? 8);
            GLFW.WindowHint(WindowHintInt.RefreshRate, -1);
        }
        else
        {
            monitor = threadMonitorRegistration.Pointer;
            var modePointer = GLFW.GetVideoMode(monitor);

            GLFW.WindowHint(WindowHintInt.RedBits, settings.RedBits ?? modePointer->RedBits);
            GLFW.WindowHint(WindowHintInt.GreenBits, settings.GreenBits ?? modePointer->GreenBits);
            GLFW.WindowHint(WindowHintInt.BlueBits, settings.BlueBits ?? modePointer->BlueBits);
            GLFW.WindowHint(WindowHintInt.BlueBits, modePointer->RefreshRate);
        }

        if (settings.AlphaBits is not null)
            GLFW.WindowHint(WindowHintInt.AlphaBits, settings.AlphaBits.Value);

        if (settings.DepthBits is not null)
            GLFW.WindowHint(WindowHintInt.DepthBits, settings.DepthBits.Value);

        if (settings.StencilBits is not null)
            GLFW.WindowHint(WindowHintInt.StencilBits, settings.StencilBits.Value);

        GLFW.WindowHint(WindowHintBool.Resizable, settings.Resizable);
        GLFW.WindowHint(WindowHintBool.TransparentFramebuffer, settings.TransparentFramebuffer);
        GLFW.WindowHint(WindowHintBool.Decorated, settings.Decorated);
        GLFW.WindowHint(WindowHintBool.Visible, settings.Visible);

        var window = GLFW.CreateWindow(
            settings.Width,
            settings.Height,
            settings.Title,
            monitor,
            share);

        if (window is null)
        {
            Terminate();
            return new WindowCreateResult(null, GLFWHelper.GetError());
        }

        return new WindowCreateResult(WindowSetup(window, settings), null);
    }

    private GlfwWindowRegistration WindowSetup(Window* window, WindowCreateSettings settings)
     {
         GLFWHelper.GetFramebufferSize(window, out var framebufferSize);
         GLFWHelper.GetWindowSize(window, out var size);
         
         var registration = new GlfwWindowRegistration
         {
             Pointer = window,
             Id = new WindowId(_nextWindowId++),
             
             Ratio = framebufferSize.AspectRatio,
             Size = size,
             FramebufferSize = framebufferSize
         };

         registration.Handle = new WindowHandle(_renderer, registration);
        
         // Setting icons
         if (settings.WindowImages != null)
             WindowSetIcons(registration, settings.WindowImages.ToList());
         
         // Setting callbacks
         GLFW.SetKeyCallback(window, _keyCallback);
         GLFW.SetWindowCloseCallback(window, _windowCloseCallback);
         GLFW.SetWindowSizeCallback(window, _windowSizeCallback);
         GLFW.SetWindowFocusCallback(window, _windowFocusCallback);
         
         return registration;
     }
     
     public void WindowDestroy(WindowRegistration registration)
     {
         if (registration is not GlfwWindowRegistration glfwRegistration)
             return;

         var window = glfwRegistration.Pointer;
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

         GLFW.DestroyWindow(window);
     }

     private bool TryGetWindow(Window* window, [NotNullWhen(true)] out GlfwWindowRegistration? registration)
     {
         registration = GetWindow(window);
         return registration is not null;
     }

     private GlfwWindowRegistration? GetWindow(Window* window)
     {
         foreach (var (_, registration) in _renderer.Windows)
         {
             if (registration is not GlfwWindowRegistration glfwRegistration)
                 continue;
             
             if (glfwRegistration.Pointer != window)
                 continue;

             return glfwRegistration;
         }

         return null;
     }

     public void WindowSetTitle(WindowRegistration window, string title)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;
        
         GLFW.SetWindowTitle(glfwWindow.Pointer, title);
     }
     
     public void WindowSetMonitor(WindowRegistration window, MonitorRegistration registration)
     {
         WindowSetMonitor(window, registration, Vector2Int.Zero);
     }
     
     public void WindowRequestAttention(WindowRegistration window)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;
        
         GLFW.RequestWindowAttention(glfwWindow.Pointer);
     }

     public void WindowSetSize(WindowRegistration window, Vector2Int size)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;
        
         GLFW.SetWindowSize(glfwWindow.Pointer, size.X, size.Y);
     }

     public void WindowSetVisible(WindowRegistration window, bool visible)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;

         if (visible)
         {
             GLFW.ShowWindow(glfwWindow.Pointer);
             return;
         }
            
         GLFW.HideWindow(glfwWindow.Pointer);
     }

     public void WindowSetOpacity(WindowRegistration window, float opacity)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;
        
         GLFW.SetWindowOpacity(glfwWindow.Pointer, opacity);
     }
     
     public void WindowSetPosition(WindowRegistration window, Vector2Int position)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;
         
         GLFW.SetWindowPos(glfwWindow.Pointer, position.X, position.Y);
     }

     public void WindowSwapBuffers(WindowRegistration window)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
             return;
        
         GLFW.SwapBuffers(glfwWindow.Pointer);
     }
     
     public void WindowSetIcons(WindowRegistration window, List<ITexture> images)
     {
         if (window is not GlfwWindowRegistration glfwWindow)
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
         
         GLFW.SetWindowIcon(glfwWindow.Pointer, glfwImages);
     }
     
     public IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureMan, IResourceManager resourceManager, ResourcePath path)
     {
         var files = resourceManager.FindContentFiles(path);
         
         foreach (var file in files)
         {
             yield return textureMan.CreateTexture(file);
         }
     }
     
     private class GlfwWindowRegistration : WindowRegistration
     {
         public Window* Pointer;
     }
}
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Hypercube.Client.Graphics.Realisation.OpenGL;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Utilities.Helpers;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwImage = OpenTK.Windowing.GraphicsLibraryFramework.Image;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace Hypercube.Client.Graphics.Windows.Realisation.GLFW;

public sealed unsafe partial class GlfwWindowManager
{
    public WindowCreateResult WindowCreate(ContextInfo? context, WindowCreateSettings settings, WindowRegistration? contextShare)
    { 
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintString.X11ClassName, "Hypercube");
        OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintString.X11InstanceName, "Hypercube");

        if (context is null)
        {
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
        }
        else
        {
#if DEBUG
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.OpenGLDebugContext, true);
#endif
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.ContextVersionMajor, context.Value.Version.Major);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintInt.ContextVersionMinor, context.Value.Version.Minor);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.OpenGLForwardCompat, context.Value.Compatibility);
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintBool.SrgbCapable, true);
            
            switch (context.Value.Profile)
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
            }
            
            OpenTK.Windowing.GraphicsLibraryFramework.GLFW.WindowHint(WindowHintContextApi.ContextCreationApi,
                context.Value.Api == ContextApi.EglContextApi
                    ? ContextApi.EglContextApi
                    : ContextApi.NativeContextApi);
        }
        
        Window* share = null;
        if (contextShare is GlfwWindowRegistration glfwShare)
            share = glfwShare.Pointer;
        
        Monitor* monitor = null;
        if (settings.Monitor is null || !_monitors.TryGetValue(settings.Monitor.Id, out var threadMonitorRegistration)) {

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
        
        var window = OpenTK.Windowing.GraphicsLibraryFramework.GLFW.CreateWindow(
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
             SetWindowIcons(registration, settings.WindowImages.ToList());
         
         // Setting callbacks
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetKeyCallback(window, _keyCallback);
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowCloseCallback(window, _windowCloseCallback);
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowSizeCallback(window, _windowSizeCallback);
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowFocusCallback(window, _windowFocusCallback);
         
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

         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.DestroyWindow(window);
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

     public IEnumerable<ITexture> LoadWindowIcons(ITextureManager textureMan, IResourceManager resourceManager, ResourcePath path)
     {
         var files = resourceManager.FindContentFiles(path);
         
         foreach (var file in files)
         {
             yield return textureMan.CreateTexture(file);
         }
     }

     public void SetWindowIcons(WindowRegistration window, List<ITexture> images)
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
         
         OpenTK.Windowing.GraphicsLibraryFramework.GLFW.SetWindowIcon(glfwWindow.Pointer, glfwImages);
     }
     
     private class GlfwWindowRegistration : WindowRegistration
     {
         public unsafe Window* Pointer;
     }
}
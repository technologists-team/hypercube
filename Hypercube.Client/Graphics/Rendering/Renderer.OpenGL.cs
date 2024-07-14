using System.Runtime.InteropServices;
using Hypercube.Client.Graphics.Event;
using Hypercube.Shared.Logging;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Rendering;

public sealed partial class Renderer
{
    private const int SwapInterval = 1;

    /// <summary>
    /// This is where we store the callback
    /// because otherwise GC will collect it.
    /// </summary>
    private DebugProc? _debugProc;
    
    private void InitOpenGL()
    {
        GL.LoadBindings(_bindingsContext);
        
        _loggerOpenGL.EngineInfo("Bindings loaded");

        var vendor = GL.GetString(StringName.Vendor);
        var renderer = GL.GetString(StringName.Renderer);
        var version = GL.GetString(StringName.Version);
        var shading = GL.GetString(StringName.ShadingLanguageVersion);
        
        _loggerOpenGL.EngineInfo($"Vendor: {vendor}");
        _loggerOpenGL.EngineInfo($"Renderer: {renderer}");
        _loggerOpenGL.EngineInfo($"Version: {version}, Shading: {shading}");
        
        GLFW.SwapInterval(SwapInterval);
        _loggerOpenGL.EngineInfo($"Swap interval: {SwapInterval}");

        _debugProc = DebugMessageCallback;
        GL.DebugMessageCallback(_debugProc, IntPtr.Zero);
        
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.DebugOutput);
        
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.ClearColor(0, 0, 0, 0);
        
        _loggerOpenGL.EngineInfo("Initialized");
        _eventBus.Raise(new GraphicsLibraryInitializedEvent());
    }

    private void DebugMessageCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr messagePointer, IntPtr userparam)
    {
        // In order to access the string pointed to by pMessage, you can use Marshal
        // class to copy its contents to a C# string without unsafe code. You can
        // also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
        string message = Marshal.PtrToStringAnsi(messagePointer, length);

        // The rest of the function is up to you to implement, however a debug output
        // is always useful.
        var logger = LoggingManager.GetLogger("open_gl_debug");
        logger.EngineInfo($"{severity} source={source} type={id} {message}");

        // Potentially, you may want to throw from the function for certain severity
        // messages.
        if (type == DebugType.DebugTypeError)
        {
            throw new Exception(message);
        }
    }
}
using System.Diagnostics;
using System.Runtime.InteropServices;
using Hypercube.Client.Graphics.Events;
using Hypercube.Shared.Logging;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    private const int SwapInterval = 1;

    /// <summary>
    /// This is where we store the callback
    /// because otherwise GC will collect it.
    /// </summary>
    private DebugProc? _debugProc;
    
    private unsafe void InitOpenGL()
    {
        GL.LoadBindings(_bindingsContext);
        
        _loggerOpenGL.EngineInfo("Bindings loaded");

        var vendor = GL.GetString(StringName.Vendor);
        var renderer = GL.GetString(StringName.Renderer);
        var version = GL.GetString(StringName.Version);
        var shading = GL.GetString(StringName.ShadingLanguageVersion);
        
        GLFW.SwapInterval(SwapInterval);
        
        _loggerOpenGL.EngineInfo($"Vendor: {vendor}");
        _loggerOpenGL.EngineInfo($"Renderer: {renderer}");
        _loggerOpenGL.EngineInfo($"Version: {version}, Shading: {shading}");
        _loggerOpenGL.EngineInfo($"Thread: {Thread.CurrentThread.Name ?? "unnamed"} ({Environment.CurrentManagedThreadId})");
        _loggerOpenGL.EngineInfo($"Swap interval: {SwapInterval}");
        
        _debugProc = DebugMessageCallback;
        
        GL.DebugMessageCallback(_debugProc, nint.Zero);
        
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
        
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.ClearColor(0, 0, 0, 0);
        
        _loggerOpenGL.EngineInfo("Initialized");
        _eventBus.Raise(new GraphicsLibraryInitializedEvent());
    }
    
    private unsafe void DebugMessageCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint messagePointer, nint @params)
    {
        var message = Marshal.PtrToStringAnsi(messagePointer, length);
        var logger = LoggingManager.GetLogger("open_gl_debug");

        var loggingMessage = $"[{type}] [{severity}] [{source}] {message} ({id})";
        
        if (type == DebugType.DebugTypeError)
        {
            logger.Fatal(loggingMessage);
            throw new Exception(message);
        }
        
        logger.EngineInfo(loggingMessage);
    }
}
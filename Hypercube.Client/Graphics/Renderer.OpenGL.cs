using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics;

public sealed partial class Renderer
{
    private const int SwapInterval = 1;
    
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
        
        _loggerOpenGL.EngineInfo("Initialized");
    }
}
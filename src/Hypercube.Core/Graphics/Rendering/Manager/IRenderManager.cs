using Hypercube.Core.Graphics.Rendering.Api;
using Hypercube.Core.Graphics.Rendering.Api.Handlers;
using Hypercube.Core.Graphics.Rendering.Api.Settings;
using Hypercube.Core.Graphics.Rendering.Shaders;
using Hypercube.Core.Windowing.Api;
using Hypercube.Core.Windowing.Windows;

namespace Hypercube.Core.Graphics.Rendering.Manager;

public interface IRenderManager
{
    event DrawHandler? OnDraw;
    
    [EngineInternal]
    IRenderingApi Api { get; }
    
    int BatchCount { get; }
    int VerticesCount { get; }
    
    FrameCounter FrameCounter { get; }
    
    void Init(IContextInfoProvider context, RenderingApiSettings settings);
    void Load();
    void Shutdown();
    void Render(IWindow window);
    
    IShaderProgram CreateShaderProgram(string source);
}
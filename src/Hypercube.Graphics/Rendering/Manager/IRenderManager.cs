using Hypercube.Graphics.Rendering.Api;
using Hypercube.Graphics.Rendering.Api.Handlers;
using Hypercube.Graphics.Rendering.Api.Settings;
using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Graphics.Windowing;

namespace Hypercube.Graphics.Rendering.Manager;

public interface IRenderManager
{
    event DrawHandler? OnDraw;
    
    void Init(IContextInfo context, RenderingApiSettings settings);
    void Load();
    void Shutdown();
    void Render(IWindow window);
    
    IShaderProgram CreateShaderProgram(string source);
}
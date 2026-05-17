using Hypercube.Core.Graphics.Rendering.Api;
using Hypercube.Core.Graphics.Rendering.Api.Handlers;
using Hypercube.Core.Graphics.Rendering.Api.Settings;
using Hypercube.Core.Graphics.Rendering.Context;
using Hypercube.Core.Graphics.Rendering.Shaders;
using Hypercube.Core.Windowing.Api;
using Hypercube.Core.Windowing.Manager;
using Hypercube.Core.Windowing.Windows;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Graphics.Rendering.Manager;

[EngineInternal]
[UsedImplicitly]
public sealed class RenderManager : IRenderManager, IRenderManagerInternal
{
    [Dependency] private readonly DependenciesContainer _dependenciesContainer = null!;
    [Dependency] private readonly IWindowingManager _windowingManager = null!;
    [Dependency] private readonly ILogger _logger = null!;
    [Dependency] private readonly IRenderContext _context = null!;

    public event DrawHandler? OnDraw;

    public IRenderingApi Api
    {
        get => field ?? throw new Exception();
        private set;
    }

    public FrameCounter FrameCounter { get; } = new();

    public int BatchCount => Api.BatchCount;
    public int VerticesCount => Api.VerticesCount;

    public void Init(IContextInfoProvider context, RenderingApiSettings settings)
    {
        Api = ApiFactory.Get(settings.Api, settings, _windowingManager.Api);
        
        _dependenciesContainer.Inject(Api);

        Api.OnInit += OnInit;
        Api.OnDebugInfo += OnDebugInfo;
        Api.OnDraw += OnDraw;
        
        Api.Init(context);
        
        _context.Init(Api, _windowingManager.Api);

        FrameCounter.Start();
    }

    private void OnInit(string info)
    {
        _logger.Info($"Render Api ({Enum.GetName(Api.Type)}) info:\r\n{info}");
    }

    private void OnDebugInfo(string info)
    {
        _logger.Debug(info);
    }

    public void Load()
    {
        Api.Load();
    }

    public void Shutdown()
    {
        Api.Terminate();
    }

    public void Render(IWindow window)
    {
        FrameCounter.Update();
        
        Api.Render(window);
    }

    public IShaderProgram CreateShaderProgram(string source)
    {
        return Api.CreateShaderProgram(source);
    }
}

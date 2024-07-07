using System.Collections.Frozen;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Client.Graphics.Windows.Manager;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Timing;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit;

namespace Hypercube.Client.Graphics;

public sealed partial class Renderer : IRenderer, IPostInject
{
    [Dependency] private readonly ITiming _timing = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly ILogger _logger = LoggingManager.GetLogger("renderer");
    private readonly ILogger _loggerOpenGL = LoggingManager.GetLogger("open_gl")!;

    private IWindowManager _windowManager = default!;
    private IBindingsContext _bindingsContext = default!;
    private Thread _currentThread = default!;

    private ContextInfo _context = default!;

    private readonly FrozenSet<ContextInfo> _contextInfos =
        new HashSet<ContextInfo>
        {
            new()
            {
                Version = new Version(4, 6),
                Profile = ContextProfile.Core,
                Api = ContextApi.NativeContextApi,
            },
            new()
            {
                Version = new Version(3, 3),
                Profile = ContextProfile.Core,
                Api = ContextApi.NativeContextApi
            },
            new() {
                Version = new Version(3, 1),
                Profile = ContextProfile.Compatability,
                Api = ContextApi.NativeContextApi
            },
            new() {
                Version = new Version(3, 0),
                Profile = ContextProfile.Any, // Because ES
                Api = OperatingSystem.IsWindows() ? ContextApi.EglContextApi : ContextApi.NativeContextApi
            },
            new() {
                Version = new Version(2, 0),
                Profile = ContextProfile.Any, // Because ES
                Api = OperatingSystem.IsWindows() ? ContextApi.EglContextApi : ContextApi.NativeContextApi
            }
        }.ToFrozenSet();

    private IShader _baseShader = default!;
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(OnInitialization);
        _eventBus.Subscribe<RuntimeStartupEvent>(OnStartup);
        _eventBus.Subscribe<UpdateFrameEvent>(OnFrameUpdate);
        _eventBus.Subscribe<RenderFrameEvent>(OnFrameRender);
    }

    private void OnInitialization(RuntimeInitializationEvent args)
    {
        _windowManager = CreateWindowManager();
        _bindingsContext = new BindingsContext(_windowManager);
    }

    private void OnStartup(RuntimeStartupEvent args)
    {
        _currentThread = Thread.CurrentThread;
        _logger.EngineInfo($"Working thread {_currentThread.Name}");

        var settings = new WindowCreateSettings();
        foreach (var contextInfo in _contextInfos)
        {
            if (!InitMainWindow(contextInfo, settings))
                continue;

            _context = contextInfo;
            _logger.EngineInfo($"Initialize main window, {contextInfo}");
            break;
        }

        InitOpenGL();

        OnLoad();
    }
    
    private IWindowManager CreateWindowManager()
    {
        var windowManager = new GlfwWindowManager();
        if (!windowManager.Init())
            throw new Exception();

        return windowManager;
    }
}
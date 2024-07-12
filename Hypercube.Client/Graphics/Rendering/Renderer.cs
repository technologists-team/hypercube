using System.Collections.Frozen;
using Hypercube.Client.Graphics.OpenGL;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Client.Graphics.Windows.Manager;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;
using Hypercube.Shared.Timing;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit;

namespace Hypercube.Client.Graphics.Rendering;

public sealed partial class Renderer : IRenderer, IPostInject, IEventSubscriber
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly ITextureManager _textureManager = default!;
    [Dependency] private readonly ITiming _timing = default!;
    [Dependency] private readonly ICameraManager _cameraManager = default!;
    [Dependency] private readonly IResourceManager _resourceManager = default!;

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
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(this, OnInitialization);
        _eventBus.Subscribe<RuntimeStartupEvent>(this, OnStartup);
        _eventBus.Subscribe<UpdateFrameEvent>(this, OnFrameUpdate);
        _eventBus.Subscribe<RenderFrameEvent>(this, OnFrameRender);
    }

    private void OnInitialization(ref RuntimeInitializationEvent args)
    {
        _windowManager = CreateWindowManager();
        _bindingsContext = new BindingsContext(_windowManager);
    }

    private void OnStartup(ref RuntimeStartupEvent args)
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

        var windowIcons = _windowManager.LoadWindowIcon(_textureManager, _resourceManager, "/Icons").ToList();
        _windowManager.SetWindowIcons(MainWindow, windowIcons);
        
        
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
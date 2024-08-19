using System.Collections.Frozen;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Client.Graphics.Windows.Realisation.GLFW;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Graphics.Texturing;
using Hypercube.Graphics.Windowing;
using Hypercube.Logging;
using Hypercube.OpenGL;
using Hypercube.Resources.Container;
using Hypercube.Resources.Manager;
using Hypercube.Runtime.Events;
using Hypercube.Shared.Timing;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer : IRenderer, IPostInject, IEventSubscriber
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly ITextureManager _textureManager = default!;
    [Dependency] private readonly ITiming _timing = default!;
    [Dependency] private readonly ICameraManager _cameraManager = default!;
    [Dependency] private readonly IResourceLoader _resourceLoader = default!;
    [Dependency] private readonly IResourceContainer _resourceContainer = default!;

    private readonly ILogger _logger = LoggingManager.GetLogger("renderer");
    private readonly ILogger _loggerOpenGL = LoggingManager.GetLogger("open_gl")!;

    private IWindowing _windowing = default!;
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
        _windowing = CreateWindowManager();
        _bindingsContext = new BindingsContext(_windowing);
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

        //var windowIcons = _windowing.LoadWindowIcons(_textureManager, _resourceLoader, "/Icons").ToList();
        //_windowing.WindowSetIcons(MainWindow, windowIcons);
        
        InitOpenGL();
        OnLoad();
    }
    
    private IWindowing CreateWindowManager()
    {
        var windowManager = new GlfwWindowing();
        if (!windowManager.Init())
            throw new Exception();

        return windowManager;
    }
}
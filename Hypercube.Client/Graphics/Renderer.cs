using System.Collections.Frozen;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Client.Graphics.Windows.Manager;
using Hypercube.Client.Runtimes.Event;
using Hypercube.Client.Runtimes.Loop.Event;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit;

namespace Hypercube.Client.Graphics;

public sealed partial class Renderer : IRenderer, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    private readonly ILogger _logger = LoggingManager.GetLogger("renderer");
    private readonly ILogger _loggerOpenGL =  LoggingManager.GetLogger("open_gl")!;
    
    private IWindowManager _windowManager = default!;
    private IBindingsContext _bindingsContext = default!;
    private Thread _currentThread = default!;

    private RendererOpenGLVersion _version = RendererOpenGLVersion.GL33;
    private readonly FrozenDictionary<RendererOpenGLVersion, ContextInfo> _contextInfos = new Dictionary<RendererOpenGLVersion, ContextInfo>
    {
        {
            RendererOpenGLVersion.GL33,
            new ContextInfo
            {
                Version = new Version(3, 3),
                Profile = ContextProfile.Core,
                Api =  ContextApi.NativeContextApi
            }
        },
        {
            RendererOpenGLVersion.GL31,
            new ContextInfo
            {
                Version = new Version(3, 1),
                Profile = ContextProfile.Compatability,
                Api =  ContextApi.NativeContextApi
            }
        },
        {
            RendererOpenGLVersion.GLES3,
            new ContextInfo
            {
                Version = new Version(3, 0),
                Profile = ContextProfile.Any, // Because ES
                Api = OperatingSystem.IsWindows() ? ContextApi.EglContextApi : ContextApi.NativeContextApi
            }
        },
        {
            RendererOpenGLVersion.GLES2,
            new ContextInfo
            {
                Version = new Version(2, 0),
                Profile = ContextProfile.Any, // Because ES
                Api = OperatingSystem.IsWindows() ? ContextApi.EglContextApi : ContextApi.NativeContextApi
            }
        }
    }.ToFrozenDictionary();
    
    public void PostInject()
    {
        _eventBus.Subscribe<InitializationEvent>(OnInitialization);
        _eventBus.Subscribe<StartupEvent>(OnStartup);
        _eventBus.Subscribe<UpdateFrameEvent>(OnFrameUpdate);
        _eventBus.Subscribe<RenderFrameEvent>(OnFrameRender);
    }

    private void OnInitialization(InitializationEvent args)
    {
        _windowManager = CreateWindowManager();
        _bindingsContext = new BindingsContext(_windowManager);
    }

    private void OnStartup(StartupEvent args)
    {
        _currentThread = Thread.CurrentThread;
        _logger.EngineInfo($"Working thread {_currentThread.Name}");
        
        var settings = new WindowCreateSettings();
        foreach (var (version, contextInfo) in _contextInfos)
        {
            if (!InitMainWindow(contextInfo, settings))
                continue;

            _version = version;
            _logger.EngineInfo($"Initialize main window, {contextInfo}");
            break;
        }
        
        InitOpenGL();
    }

    private void OnFrameUpdate(UpdateFrameEvent args)
    {
        _windowManager.PollEvents();
    }

    private void OnFrameRender(RenderFrameEvent args)
    {

    }
    
    private IWindowManager CreateWindowManager()
    {
        var windowManager = new GlfwWindowManager();
        if (!windowManager.Init())
            throw new Exception();

        return windowManager;
    }
}
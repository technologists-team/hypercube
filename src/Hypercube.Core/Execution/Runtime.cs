using System.Reflection;
using Hypercube.Core.Ecs;
using Hypercube.Core.Ecs.Core.Systems;
using Hypercube.Core.Execution.Attributes;
using Hypercube.Core.Execution.Enums;
using Hypercube.Core.Utilities.Helpers;
using Hypercube.Graphics.Rendering;
using Hypercube.Graphics.Windowing.Settings;
using Hypercube.Resources.Loader;
using Hypercube.Utilities.Configuration;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;
using Hypercube.Utilities.Extensions;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.Execution;

public sealed class Runtime
{
    private readonly DependenciesContainer _dependencies = new();

    [Dependency] private readonly IConfigManager _configManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly IRuntimeLoop _runtimeLoop = default!;
    [Dependency] private readonly IResourceLoader _resourceLoader = default!;
    [Dependency] private readonly IRenderer _renderer = default!;

    private readonly ILogger _logger = new ConsoleLogger();
    private readonly Dictionary<EntryPointLevel, List<MethodInfo>> _entryPoints = [];
    
    public void Start()
    {
        Thread.CurrentThread.Name = "Main";
        
        InitPrimaryDependencies();
        
        _logger.Echo(EngineInfo.WelcomeMessage);
        _logger.Info("Dependency initialization...");
        
        InitDependencies();

        _configManager.Init();
        
        _logger.Info("Dependency initialization is complete!");
        _logger.Info("Preparing for the execution of entry points...");

        EntryPointsLoad();
        EntryPointsExecute(EntryPointLevel.BeforeInit);

        foreach (var (file, prefix) in Config.MountFolders.Value)
        {
            _resourceLoader.MountContentFolder(file, prefix);
        }
        
        _logger.Info("The entry points are called!");
        _logger.Info("Initialization of internal modules...");
        
        _entitySystemManager.CrateMainWorld();
        
        _renderer.Init(new RendererSettings
        {
            Thread = WindowingThreadSettingsHelper.FromConfig(),
            WindowingApi = WindowingApiSettingsHelper.FromConfig(),
            RenderingApi = RenderingApiSettingsHelper.FromConfig(),
            ReadySleepDelay = Config.WindowingThreadReadySleepDelay
        });
        
        _renderer.CreateMainWindow(new WindowCreateSettings
        {
            Api = new ApiSettings
            {
                Api = ContextApi.OpenGl,
                Flags = ContextFlags.Debug,
                Profile = ContextProfile.Core,
                Version = new Version(4, 6)
            },
            Title = Config.MainWindowTitle,
            Resizable = Config.MainWindowResizable,
            Decorated = Config.MainWindowDecorated,
            Floating = Config.MainWindowFloating,
            Visible = Config.MainWindowVisible,
            TransparentFramebuffer = Config.MainWindowTransparentFramebuffer,
        });
        
        _renderer.Load();
        
        _logger.Info("Preparation is complete, start the main application cycle");
        EntryPointsExecute(EntryPointLevel.AfterInit);
        _runtimeLoop.Run();
    }

    private void InitPrimaryDependencies()
    {
        _dependencies.Register<ILogger>(_logger);
    }
    
    private void InitDependencies()
    {
        _dependencies.Register<IConfigManager, ConfigManager>();
        _dependencies.Register<IRuntimeLoop, RuntimeLoop>();
        _dependencies.Register<IEntitySystemManager, EntitySystemManager>();

        Resources.Dependencies.Register(_dependencies);
        Graphics.Dependencies.Register(_dependencies);

        _dependencies.InstantiateAll();
        _dependencies.Inject(this);
    }

    private void EntryPointsLoad()
    {
        var methods = ReflectionHelper.GetExecutableMethodsWithAttributeFromAllAssemblies<EntryPointAttribute>(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (var method in methods)
        {
            if (!method.IsStatic)
                throw new InvalidOperationException();
            
            var attribute = method.GetCustomAttribute<EntryPointAttribute>()!;
            _entryPoints.GetOrInstantiate(attribute.Level).Add(method);
            
            _logger.Debug($"Loaded {method.Name} entry point");
        }
    }
    
    private void EntryPointsExecute(EntryPointLevel level)
    {
        if (!_entryPoints.TryGetValue(level, out var entryPoints))
            return;
        
        foreach (var method in entryPoints)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 1)
            {
                if (parameters[0].ParameterType == typeof(DependenciesContainer))
                {
                    method.Invoke(null, [_dependencies]);
                    continue;
                }
                
                throw new InvalidOperationException();
            }
            
            method.Invoke(null, null);
        }
    }
}
using Hypercube.Core.Audio.Manager;
using Hypercube.Core.Ecs;
using Hypercube.Core.Execution.Enums;
using Hypercube.Core.Execution.LifeCycle;
using Hypercube.Core.Execution.Timing;
using Hypercube.Core.Graphics.Rendering;
using Hypercube.Core.Resources;
using Hypercube.Core.Utilities.Extensions;
using Hypercube.Utilities.Configuration;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Execution;

public sealed partial class Runtime
{
    private readonly DependenciesContainer _dependencies = new();

    [Dependency] private readonly IAudioManager _audioManager = null!;
    [Dependency] private readonly IConfigManager _configManager = null!;
    [Dependency] private readonly IResourceManager _resourceManager = null!;
    [Dependency] private readonly IRenderer _renderer = null!;
    [Dependency] private readonly IRuntimeLoop _runtimeLoop = null!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = null!;
    [Dependency] private readonly ITime _time = null!;

    private readonly Debugging.Logger _logger = new();

    public void Start(string[] args) 
    {
        RuntimeExceptionHandling.RunClassConstructor();
        
        _parser.Parse(args);
        
        InitCore();
        InitConfig();
        InitModules();
        
        RunApplication();
        ShutdownApplication();;
    }
    
    private void InitCore()
    {
        Thread.CurrentThread.Name = "Main";
        InitPrimaryDependencies();
        
        _logger.LogLevel = Config.LoggingLevel;
        _logger.Echo(EngineInfo.WelcomeMessage);
        _logger.Info("Dependency initialization...");
        
        InitDependencies();
    }
    
    private void InitConfig()
    {
        if (!_parser.Get<bool>(RuntimeArguments.ConfigDontInit))
            _configManager.Init();

        _logger.Info("Dependency initialization is complete!");
        _logger.Info("Preparing for the execution of entry points...");

        EntryPointsLoad();
        EntryPointsExecute(EntryPointStage.BeforeInit);
    }
    
    private void InitModules()
    {
        _time.SetTickRate(Config.Tickrate);
        _time.Start();
        
        _logger.Info($"Time started. Tickrate: {Config.Tickrate.Value}");

        _resourceManager.Mount(Config.MountFolders);
        
        _logger.Info("The entry points are called!");
        _logger.Info("Initialization of internal modules...");
         
        EntryPointsExecute(EntryPointStage.BeforeRenderInit);

        _renderer.Init();
        var mainWindow = _renderer.CreateMainWindow();
        mainWindow.OnClose += () => _runtimeLoop.Shutdown();

        _audioManager.Initialize();
        _resourceManager.AddAllLoaders();
        _renderer.Load();

        InitDependentsDependencies();

        EntryPointsExecute(EntryPointStage.BeforeEntityInitialization);
        _entitySystemManager.Initialize();
        
        _logger.Info("Preparation is complete, start the main application cycle");
    }
    
    private void RunApplication()
    {
        EntryPointsExecute(EntryPointStage.AfterInit);
        _runtimeLoop.Run();
    }

    private void ShutdownApplication()
    {
        _renderer.Shutdown();
    }

    public void HandleException(Exception exception)
    {
        _logger.Critical(exception, exception.Message);
    }
}
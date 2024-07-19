using System.Diagnostics;
using Hypercube.Client.Audio.Events;
using Hypercube.Client.Audio.Resources;
using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Resources.Caching;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources.Container;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Resources.Preloader;

namespace Hypercube.Client;

public sealed class Preloader : IPreloader
{
    [Dependency] private readonly IResourceLoader _resourceLoader = default!;
    [Dependency] private readonly IResourceContainer _resourceContainer = default!;
    
    private readonly ILogger _logger = new Logger("preloader");

    [Preloading(typeof(GraphicsLibraryInitializedEvent))]
    private void PreloadGraphics()
    {
        PreloadShaders();
        PreloadTextures();
    }
    
    private void PreloadTextures()
    {
        _logger.EngineInfo("Preloading textures...");
        var stopwatch = Stopwatch.StartNew();
        var count = 0;
        
        foreach (var path in _resourceLoader.FindContentFiles("/Textures/"))
        {
            if (path.Extension != ".png")
                continue;
            
            if (_resourceContainer.Cached<TextureResource>(path))
                continue;
            
            var resource = new TextureResource(path);
            _resourceContainer.CacheResource(path, resource);
            count++;
        }
        
        stopwatch.Stop();
        _logger.EngineInfo($"Preloaded {count} textures in {stopwatch.Elapsed}");
    }

    private void PreloadShaders()
    {
        _logger.EngineInfo("Preloading shaders...");
        var stopwatch = Stopwatch.StartNew();
        var count = 0;
        
        foreach (var path in _resourceLoader.FindContentFiles("/Shaders/"))
        {
            if (path.Extension != ".frag" && path.Extension != ".vert")
                continue;
            
            var basePath = $"{path.ParentDirectory}/{path.Filename}";
            if (_resourceContainer.Cached<ShaderSourceResource>(basePath))
                continue;
            
            var resource = new ShaderSourceResource(basePath);
            _resourceContainer.CacheResource(basePath, resource);
            count++;
        }
        
        stopwatch.Stop();
        _logger.EngineInfo($"Preloaded {count} shaders in {stopwatch.Elapsed}");
    }
    
    [Preloading(typeof(AudioLibraryInitializedEvent))]
    private void PreloadAudio()
    {
        _logger.EngineInfo("Preloading shaders...");
        var stopwatch = Stopwatch.StartNew();
        var count = 0;

        foreach (var path in _resourceLoader.FindContentFiles("/Audio/"))
        {
            if (path.Extension != ".wav" && path.Extension != ".ogg")
                continue;
            
            if (_resourceContainer.Cached<AudioResource>(path))
                continue;
            
            var resource = new AudioResource(path);
            _resourceContainer.CacheResource(path, resource);
            count++;
        }
        
        stopwatch.Stop();
        _logger.EngineInfo($"Preloaded {count} audio files in {stopwatch.Elapsed}");
    }
}
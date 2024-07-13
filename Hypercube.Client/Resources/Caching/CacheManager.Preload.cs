using System.Diagnostics;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;

namespace Hypercube.Client.Resources.Caching;

public partial class CacheManager
{
    public void PreloadTextures()
    {
        var logger = LoggingManager.GetLogger("cache.preload");
        
        PreloadTextures(logger);
    }
    private void PreloadTextures(Logger logger)
    {
        var container = DependencyManager.GetContainer();
        logger.EngineInfo("Preloading textures...");
        var st = Stopwatch.StartNew();

        var texDict = GetTypeDict<TextureResource>();

        var files = _resourceManager.FindContentFiles("/Textures/")
            .Where(p => !texDict.ContainsKey(p) && p.Extension == ".png")
            .Select(p => new TextureResource() { Path = p});

        
        foreach (var file in files)
        {
            file.Load(file.Path, container);
            
            texDict[file.Path] = file;
        }
    }
}
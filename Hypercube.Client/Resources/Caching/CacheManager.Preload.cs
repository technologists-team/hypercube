using System.Diagnostics;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;

namespace Hypercube.Client.Resources.Caching;

public partial class CacheManager
{
    public void PreloadTextures()
    {
        var logger = LoggingManager.GetLogger("cache.preload");
        var container = DependencyManager.GetContainer();
        PreloadTextures(logger, container);
        PreloadShaders(logger, container);
        PreloadAudio(logger, container);
    }
    private void PreloadTextures(Logger logger, DependenciesContainer container)
    {
        logger.EngineInfo("Preloading textures...");
        var st = Stopwatch.StartNew();

        var texDict = GetTypeDict<TextureResource>();

        var files = _resourceManager.FindContentFiles("/Textures/")
            .Where(p => !texDict.ContainsKey(p) && p.Extension == ".png")
            .Select(p => new TextureResource { Path = p});

        // TODO: Find a way of making Parallel.ForEach, currently it causes AccessViolation ex
        var count = 0;
        foreach (var file in files)
        {
            file.Load(file.Path, container);
            texDict[file.Path] = file;
            count++;
        }
        st.Stop();
        _logger.EngineInfo($"Preloaded {count} textures in {st.Elapsed}");
    }

    private void PreloadShaders(Logger logger, DependenciesContainer container)
    {
        logger.EngineInfo("Preloading shaders...");
        var st = Stopwatch.StartNew();

        var shDict = GetTypeDict<ShaderSourceResource>();

        var files = _resourceManager.FindContentFiles("/Shaders/")
            .Where(p => !shDict.ContainsKey(p) && p.Extension == ".vert")
            .Select(p => new ShaderSourceResource { Base = $"{p.ParentDirectory}/{p.Filename}", VertexPath = $"{p.ParentDirectory}/{p.Filename}.vert", FragmentPath = $"{p.ParentDirectory}/{p.Filename}.frag"});
        
        var count = 0;
        // TODO: Find a way of making Parallel.ForEach, currently it causes AccessViolation ex
        foreach (var file in files)
        {
            file.Load(file.Base, container);
            shDict[file.Base] = file;
            count++;
        }
        st.Stop();
        _logger.EngineInfo($"Preloaded {count} shaders in {st.Elapsed}");
    }

    private void PreloadAudio(Logger logger, DependenciesContainer container)
    {
        logger.EngineInfo("Preloading shaders...");
        var st = Stopwatch.StartNew();
        
        var aDict = GetTypeDict<ShaderSourceResource>();

        var files = _resourceManager.FindContentFiles("/Audio/")
            .Where(p => !aDict.ContainsKey(p) && p.Extension == ".wav" || p.Extension == ".ogg")
            .Select(p => new AudioSourceResource() {Path = p});

        var count = 0;
        foreach (var file in files)
        {
            file.Load(file.Path, container);
            aDict[file.Path] = file;
            count++;
        }
        st.Stop();
        _logger.EngineInfo($"Preloaded {count} audio files in {st.Elapsed}");
    }
}
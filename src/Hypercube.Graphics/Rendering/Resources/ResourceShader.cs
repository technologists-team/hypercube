using System.Collections.Frozen;
using Hypercube.Graphics.Rendering.Manager;
using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Resources;
using Hypercube.Resources.Loader;
using Hypercube.Resources.Loaders;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Graphics.Rendering.Resources;

public class ResourceShader : Resource, IDisposable
{
    public static readonly FrozenDictionary<ShaderType, string> Extension = new Dictionary<ShaderType, string>
    {
        { ShaderType.Vertex, ".vert" },
        { ShaderType.Fragment, ".frag" },
        { ShaderType.Geometry, ".geom" },
        { ShaderType.Compute, ".comp" },
        { ShaderType.Tesselation, ".tess" },
    }.ToFrozenDictionary();

    private static IResourceLoader _resourceLoader = default!;
    private static IRenderManager _renderManager = default!;
    
    public IShaderProgram? ShaderProgram { get; private set; }

    public override void Init(DependenciesContainer container)
    {
        _resourceLoader = container.Resolve<IResourceLoader>();
        _renderManager = container.Resolve<IRenderManager>();
    }

    protected override void OnLoad(ResourcePath path)
    {
        var sources = new Dictionary<ShaderType, string>();
        foreach (var (type, extension) in Extension)
        {
            var shaderPath = $"{path}{extension}";
            if (!_resourceLoader.TryReadFileContentAll(shaderPath, out var content))
                continue;
            
            sources.Add(type, content);
        }
        
        ShaderProgram = _renderManager.CreateShaderProgram(sources);
    }

    public void Dispose()
    {
        ShaderProgram?.Dispose();
    }
}
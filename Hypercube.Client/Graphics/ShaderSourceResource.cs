using Hypercube.Dependencies;
using Hypercube.Graphics.Shaders;
using Hypercube.OpenGL.Shaders;
using Hypercube.Resources;
using Hypercube.Resources.Manager;

namespace Hypercube.Client.Graphics;

public sealed class ShaderSourceResource : Resource, IDisposable
{
    public IShaderProgram ShaderProgram = default!;
    
    public string Base;
    public ResourcePath VertexPath;
    public ResourcePath FragmentPath;

    public ShaderSourceResource()
    {
        Base = string.Empty;
        VertexPath = string.Empty;
        FragmentPath = string.Empty;
    }
    
    public ShaderSourceResource(ResourcePath path)
    {
        Base = path;
        VertexPath = $"{path}.vert";
        FragmentPath =  $"{path}.frag";
    }
    
    protected override void OnLoad(ResourcePath path, DependenciesContainer container)
    {
        var resourceLoader = container.Resolve<IResourceLoader>();
        var vertSource = resourceLoader.ReadFileContentAllText($"{path}.vert");
        var fragSource = resourceLoader.ReadFileContentAllText($"{path}.frag");
        
        ShaderProgram = new ShaderProgram(vertSource, fragSource);
    }

    public void Dispose()
    {
        ShaderProgram.Dispose();
    }
}
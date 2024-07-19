using Hypercube.Client.Graphics.Realisation.OpenGL.Shaders;
using Hypercube.Client.Graphics.Shaders;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Resources.Caching;

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
        ShaderProgram = new ShaderProgram(path, container.Resolve<IResourceLoader>());
    }

    public void Dispose()
    {
        ShaderProgram.Dispose();
    }
}
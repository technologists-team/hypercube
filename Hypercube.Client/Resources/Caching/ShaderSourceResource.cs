using Hypercube.Client.Graphics.Realisation.OpenGL.Shaders;
using Hypercube.Client.Graphics.Shaders;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching.Resource;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Resources.Caching;

public sealed class ShaderSourceResource : Resource, IDisposable
{
    public IShaderProgram ShaderProgram;
    public string Base;
    public ResourcePath VertexPath;
    public ResourcePath FragmentPath;
    
    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        ShaderProgram = new ShaderProgram(path, container.Resolve<IResourceManager>());
    }

    public void Dispose()
    {
        ShaderProgram.Dispose();
    }
}
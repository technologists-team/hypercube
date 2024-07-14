using Hypercube.Client.Graphics.Shading;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching.Resource;
using Hypercube.Shared.Resources.Manager;

namespace Hypercube.Client.Resources.Caching;

public class ShaderSourceResource : BaseResource, IDisposable
{
    public IShader Shader;
    public string Base;
    public ResourcePath VertexPath;
    public ResourcePath FragmentPath;
    
    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        Shader = new Shader(path, container.Resolve<IResourceManager>());
    }

    public void Dispose()
    {
        Shader.Dispose();
    }
}
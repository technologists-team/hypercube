using Hypercube.Graphics.Rendering.Manager;
using Hypercube.Resources;
using Hypercube.Resources.FileSystems;
using Hypercube.Resources.Loaders;

namespace Hypercube.Graphics;

public sealed class ShaderResourceLoader : ResourceLoader<Shader>
{
    private readonly IRenderManager _renderManager;

    public override string[] Extensions => ["shd"];

    public ShaderResourceLoader(IRenderManager renderManager)
    {
        _renderManager = renderManager;
    }

    public override bool CanLoad(ResourcePath path, IFileSystem fileSystem)
    {
        return true;
    }

    public override Shader Load(ResourcePath path, IFileSystem fileSystem)
    {
        return new Shader(_renderManager.CreateShaderProgram(fileSystem.OpenRead(path).ReadToEnd()));
    }
}
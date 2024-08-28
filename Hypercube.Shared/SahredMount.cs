using Hypercube.Dependencies;
using Hypercube.Resources.Manager;

namespace Hypercube.Shared;

public static class MountFolders
{
    public static void Mount(DependenciesContainer rootContainer)
    {
        var resourceLoader = rootContainer.Resolve<IResourceLoader>();
        
        resourceLoader.MountContentFolder(".", "/");
        resourceLoader.MountContentFolder("Resources", "/");
        resourceLoader.MountContentFolder("Resources/Audio", "/");
        resourceLoader.MountContentFolder("Resources/Textures", "/");
        resourceLoader.MountContentFolder("Resources/Shaders", "/");
    }
}
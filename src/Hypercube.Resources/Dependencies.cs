using Hypercube.Utilities.Dependencies;

namespace Hypercube.Resources;

public static class Dependencies
{
    public static void Register(DependenciesContainer container)
    {
        container.Register<IResourceManager>(new ResourceManager());
    }
}
namespace Hypercube.Core.Ecs.Systems;

public class EntitySystemManager : IEntitySystemManager
{
    private readonly List<World> _worlds = [];

    public World CreateWorld()
    {
        return new World();
    }

    public IEntitySystem GetSystem<T>() where T : IEntitySystem
    {
        throw new NotImplementedException();
    }
}   
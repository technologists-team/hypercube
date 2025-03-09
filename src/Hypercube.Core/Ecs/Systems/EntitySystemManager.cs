namespace Hypercube.Core.Ecs.Systems;

public class EntitySystemManager : IEntitySystemManager
{
    private readonly List<World> _worlds = [];
    
    public IWorld GetWorld(int id)
    {
        throw new NotImplementedException();
    }
}   
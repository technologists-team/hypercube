using Hypercube.Core.Ecs.Utilities;

namespace Hypercube.Core.Ecs.Systems;

public sealed class EntitySystemManager : IEntitySystemManager
{
    public IWorld Main { get; }

    private readonly IntPool _worldIdPool = new();
    
    private IWorld[] _worlds = [];

    public EntitySystemManager()
    {
        Main = CreateWorld();
    }

    public IWorld CreateWorld()
    {
        var world = new World(_worldIdPool.Next);
        
        if (_worlds.Length >= world.Id)
            Array.Resize(ref _worlds, world.Id + 1);

        _worlds[world.Id] = world;
        
        return world;
    }
}
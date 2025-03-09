namespace Hypercube.Core.Ecs.Systems;

public class EntitySystemManager : IEntitySystemManager
{
    private readonly World _world;

    public EntitySystemManager()
    {
        _world = new World();
        
        var registrar = new WorldRegistrar(_world);
        registrar.Register();
    }
}   
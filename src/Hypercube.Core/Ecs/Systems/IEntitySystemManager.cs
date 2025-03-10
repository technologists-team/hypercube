namespace Hypercube.Core.Ecs.Systems;

public interface IEntitySystemManager
{
    IWorld Main { get; }

    void Update(float deltaTime);
    
    void CrateMainWorld();
    IWorld CreateWorld();
}
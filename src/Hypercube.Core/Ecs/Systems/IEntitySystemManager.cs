namespace Hypercube.Core.Ecs.Systems;

public interface IEntitySystemManager
{
    IWorld Main { get; }

    void CrateMainWorld();
    IWorld CreateWorld();
}
namespace Hypercube.Core.Ecs.Systems;

public interface IEntitySystemManager
{
    IWorld Main { get; }

    IWorld CreateWorld();
}
namespace Hypercube.Core.Ecs.Systems;

public interface IEntitySystemManager
{
    IWorld GetWorld(int id);
}
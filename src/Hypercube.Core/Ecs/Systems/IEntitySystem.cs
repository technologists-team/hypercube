namespace Hypercube.Core.Ecs.Systems;

public interface IEntitySystem
{
    void Startup();
    void Shutdown();
    void Update(float deltaTime);
}
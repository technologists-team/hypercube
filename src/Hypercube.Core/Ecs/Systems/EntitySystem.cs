using Hypercube.Core.Ecs.Components;

namespace Hypercube.Core.Ecs.Systems;

public abstract class EntitySystem : IEntitySystem
{
    public World World { get; private set; }
    
    public void Startup()
    {
        throw new NotImplementedException();
    }

    public void Shutdown()
    {
        throw new NotImplementedException();
    }

    public void Update(float deltaTime)
    {
        throw new NotImplementedException();
    }
}
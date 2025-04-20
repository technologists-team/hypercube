using Hypercube.Core.Ecs.Core.Utilities;
using Hypercube.Core.Ecs.Utilities;
using Hypercube.Utilities.Dependencies;
using JetBrains.Annotations;

namespace Hypercube.Core.Ecs.Core.Systems;

[EngineInternal]
[UsedImplicitly]
public sealed class EntitySystemManager : IEntitySystemManager
{
    [Dependency] private readonly DependenciesContainer _container = default!;

    public IWorld Main { get; private set; } = default!;

    private readonly WorldRegistrar _registrar = new();
    private readonly IntPool _worldIdPool = new();

    private IWorld[] _worlds = [];

    public void Update(float deltaTime)
    {
        foreach (var world in _worlds)
            world.Update(deltaTime);
    }

    public void CrateMainWorld()
    {
        if (Main is not null)
            throw new InvalidOperationException();

        Main = CreateWorld();
    }

    public IWorld CreateWorld()
    {
        var world = new World(_worldIdPool.Next, _registrar.GetTypes(), _container);
        
        if (_worlds.Length >= world.Id)
            Array.Resize(ref _worlds, world.Id + 1);

        _worlds[world.Id] = world;
        
        return world;
    }
}
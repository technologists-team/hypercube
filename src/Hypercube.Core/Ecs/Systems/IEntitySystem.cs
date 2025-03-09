using JetBrains.Annotations;

namespace Hypercube.Core.Ecs.Systems;

public interface IEntitySystem
{
    /// <remarks>
    /// This must have a private setter,
    /// we just can't provide it in the interface.
    /// </remarks>
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    World World { get; }
    
    void Startup();
    void Shutdown();
    void Update(float deltaTime);
}
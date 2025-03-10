using Hypercube.Core.Ecs.Core;
using JetBrains.Annotations;

namespace Hypercube.Core.Ecs;

public interface IEntitySystem
{
    /// <summary>
    /// Gets the <see cref="World"/> instance that this system operates within.
    /// This property is implicitly assigned and should not be null after initialization.
    /// </summary>
    /// <remarks>
    /// This must have a private setter,
    /// we just can't provide it in the interface.
    /// </remarks>
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    World World { get; }

    /// <summary>
    /// Called when the system is started. Override this method to perform initialization logic.
    /// </summary>
    void Startup();
    
    /// <summary>
    /// Called when the system is shut down. Override this method to perform cleanup logic.
    /// </summary>
    void Shutdown();
    
    /// <summary>
    /// Called every frame or update cycle. Override this method to implement system-specific update logic.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last update, in seconds.</param>
    void Update(float deltaTime);
}
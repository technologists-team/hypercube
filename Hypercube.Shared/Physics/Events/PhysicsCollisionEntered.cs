using Hypercube.EventBus.Events;

namespace Hypercube.Shared.Physics.Events;

public readonly record struct PhysicsCollisionEntered(Manifold Manifold) : IEventArgs;
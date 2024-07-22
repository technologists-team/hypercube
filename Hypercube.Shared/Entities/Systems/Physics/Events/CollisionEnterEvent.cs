using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;

namespace Hypercube.Shared.Entities.Systems.Physics.Events;

public readonly record struct CollisionEnterEvent(PhysicsComponent Other) : IEntitiesEventArgs;
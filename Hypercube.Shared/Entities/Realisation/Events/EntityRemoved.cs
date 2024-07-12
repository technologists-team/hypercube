using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct EntityRemoved(EntityUid EntityUid) : IEventArgs;
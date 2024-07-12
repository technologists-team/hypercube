using Hypercube.Shared.EventBus.Events.Events;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct EntityRemoved(EntityUid EntityUid) : IEventArgs;
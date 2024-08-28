using Hypercube.EventBus.Events;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct EntityRemoved(EntityUid EntityUid) : IEventArgs, IEntitiesEventArgs;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct ComponentRemoved(EntityUid EntityUid, IComponent Component) : IEventArgs, IEntitiesEventArgs;
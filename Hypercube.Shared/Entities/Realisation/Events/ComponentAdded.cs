using Hypercube.EventBus.Events;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct ComponentAdded(EntityUid EntityUid, IComponent Component) : IEventArgs, IEntitiesEventArgs;
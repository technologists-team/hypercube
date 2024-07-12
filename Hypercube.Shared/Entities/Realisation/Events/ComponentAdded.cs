using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct ComponentAdded(EntityUid EntityUid, IComponent Component) : IEventArgs;
using Hypercube.Shared.Entities.Realisation.Components;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct ComponentAdded(EntityUid EntityUid, IComponent Component);
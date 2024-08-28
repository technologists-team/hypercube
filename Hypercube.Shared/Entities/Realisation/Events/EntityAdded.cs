using Hypercube.EventBus.Events;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;

namespace Hypercube.Shared.Entities.Realisation.Events;

public readonly record struct EntityAdded(EntityUid EntityUid, string Name, SceneCoordinates Coordinates) : IEventArgs, IEntitiesEventArgs;
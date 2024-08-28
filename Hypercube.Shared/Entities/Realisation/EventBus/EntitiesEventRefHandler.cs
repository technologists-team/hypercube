using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;
using Hypercube.Utilities.Units;

namespace Hypercube.Shared.Entities.Realisation.EventBus;

public delegate void EntitiesEventRefHandler(EntityUid entityUid, IComponent component, ref Unit unit);
public delegate void EntitiesEventRefHandler<TComponent, TEvent>(Entity<TComponent> entity, ref TEvent args)
    where TComponent : IComponent
    where TEvent : IEntitiesEventArgs;
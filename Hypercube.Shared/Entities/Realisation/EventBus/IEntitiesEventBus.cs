using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.EventBus.EventArgs;
using Hypercube.Shared.Entities.Realisation.Systems;

namespace Hypercube.Shared.Entities.Realisation.EventBus;

public interface IEntitiesEventBus
{
    void Raise<TEvent>(EntityUid entityUid, IComponent component, TEvent args)
        where TEvent : IEntitiesEventArgs;
    void Raise<TComponent, TEvent>(Entity<TComponent> entity, TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs;
    void Raise<TComponent, TEvent>(Entity<TComponent> entity, ref TEvent args)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs;
    void Subscribe<TComponent, TEvent>(IEntitySystem subscriber, EntitiesEventRefHandler<TComponent, TEvent> handler)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs;
    void Unsubscribe<TComponent, TEvent>(IEntitySystem subscriber)
        where TComponent : IComponent
        where TEvent : IEntitiesEventArgs;
}
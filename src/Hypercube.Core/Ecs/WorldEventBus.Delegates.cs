using Hypercube.Core.Ecs.Components;
using Hypercube.Core.Ecs.Events;
using Hypercube.Utilities.References;

namespace Hypercube.Core.Ecs;

public delegate void GlobalEventRefHandler(ref Unit unit);
public delegate void GlobalEventRefHandler<TEvent>(ref TEvent args)
    where TEvent : IEvent;

public delegate void ComponentEventRefHandler(ref IComponent component, ref Unit unit);
public delegate void ComponentEventRefHandler<TComp, TEvent>(ref TComp component, ref TEvent args)
    where TComp : IComponent where TEvent : IEvent;

public delegate void EventRefHandler(ref Entity entity, ref IComponent component, ref Unit unit);
public delegate void EventRefHandler<TComp, TEvent>(ref Entity entity, ref TComp component, ref TEvent args)
    where TComp : IComponent where TEvent : IEvent;
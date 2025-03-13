using Hypercube.Core.Analyzers;
using Hypercube.Utilities.References;

namespace Hypercube.Core.Ecs.Core.Events;

[EngineCore]
public delegate void GlobalEventRefHandler(ref Unit unit);

[EngineCore]
public delegate void GlobalEventRefHandler<TEvent>(ref TEvent args)
    where TEvent : IEvent;

[EngineCore]
public delegate void ComponentEventRefHandler(ref IComponent component, ref Unit unit);

[EngineCore]
public delegate void ComponentEventRefHandler<TComp, TEvent>(ref TComp component, ref TEvent args)
    where TComp : IComponent where TEvent : IEvent;

[EngineCore]
public delegate void EventRefHandler(ref Entity entity, ref IComponent component, ref Unit unit);

[EngineCore]
public delegate void EventRefHandler<TComp, TEvent>(ref Entity entity, ref TComp component, ref TEvent args)
    where TComp : IComponent where TEvent : IEvent;
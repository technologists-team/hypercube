using Hypercube.Shared.EventBus.Events.Events;

namespace Hypercube.Shared.Runtimes.Event;

public readonly record struct RuntimeShutdownEvent(string Reason) : IEventArgs;
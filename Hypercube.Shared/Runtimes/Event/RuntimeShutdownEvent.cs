using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Runtimes.Event;

public readonly record struct RuntimeShutdownEvent(string Reason) : IEventArgs;
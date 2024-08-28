using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.EventBus;

[PublicAPI]
public delegate void EventRefHandler<T>(ref T args)
    where T : IEventArgs;
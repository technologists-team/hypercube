using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Runtime.Events;

[PublicAPI]
public readonly record struct RuntimeInitializationEvent : IEventArgs;
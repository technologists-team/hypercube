using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.Audio.Events;

[PublicAPI]
public readonly record struct AudioLibraryInitializedEvent : IEventArgs;
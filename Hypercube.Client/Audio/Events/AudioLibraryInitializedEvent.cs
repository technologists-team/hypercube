using Hypercube.EventBus.Events;

namespace Hypercube.Client.Audio.Events;

public readonly record struct AudioLibraryInitializedEvent : IEventArgs;
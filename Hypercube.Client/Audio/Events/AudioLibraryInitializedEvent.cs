using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Audio.Events;

public readonly record struct AudioLibraryInitializedEvent : IEventArgs;
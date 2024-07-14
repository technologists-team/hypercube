using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Audio.Event;

public readonly record struct AudioLibraryInitializedEvent : IEventArgs;
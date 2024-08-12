using Hypercube.Input;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Input.Events.Windowing;

public readonly record struct WindowingKeyHandledEvent(KeyStateChangedArgs State) : IEventArgs;
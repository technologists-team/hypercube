using Hypercube.Client.Graphics.Windows;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Graphics.Event;

public readonly record struct WindowFocusChangedEvent(WindowRegistration Registration, bool Focused) : IEventArgs;
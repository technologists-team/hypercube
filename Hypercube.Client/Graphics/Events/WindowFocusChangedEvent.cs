using Hypercube.Graphics.Windowing;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Graphics.Events;

public readonly record struct WindowFocusChangedEvent(WindowHandle Handle, bool Focused) : IEventArgs;
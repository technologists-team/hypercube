using Hypercube.EventBus.Events;
using Hypercube.Graphics.Windowing;

namespace Hypercube.Client.Graphics.Events;

public readonly record struct WindowFocusChangedEvent(WindowHandle Handle, bool Focused) : IEventArgs;
using Hypercube.Input;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Client.Input.Events.Windowing;

public class WindowingMouseButtonHandledEvent : MouseButtonChangedArgs, IEventArgs
{
    public WindowingMouseButtonHandledEvent(MouseButton button, KeyState state, KeyModifiers modifiers) : base(button, state, modifiers)
    {
    }
}
using Hypercube.EventBus.Events;
using Hypercube.Input;

namespace Hypercube.Client.Input.Events;

public class MouseButtonHandledEvent : MouseButtonChangedArgs, IEventArgs
{
    public MouseButtonHandledEvent(MouseButton button, KeyState state, KeyModifiers modifiers) : base(button, state, modifiers)
    {
    }
}
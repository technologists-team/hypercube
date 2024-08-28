using Hypercube.EventBus.Events;
using Hypercube.Input;

namespace Hypercube.Client.Input.Events;

public sealed class KeyHandledEvent : KeyStateChangedArgs, IEventArgs
{
    public KeyHandledEvent(Key key, KeyState state, KeyModifiers modifiers, int scanCode) : base(key, state, modifiers, scanCode)
    {
    }
}
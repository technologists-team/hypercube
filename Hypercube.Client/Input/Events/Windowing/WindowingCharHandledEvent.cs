using Hypercube.EventBus.Events;

namespace Hypercube.Client.Input.Events.Windowing;

public class WindowingCharHandledEvent : IEventArgs
{
    public readonly uint Code;
    
    public WindowingCharHandledEvent(uint code)
    {
        Code = code;
    }
}
using Hypercube.Shared.EventBus.Events;
using EventArgs = Hypercube.Shared.EventBus.Events.EventArgs;

namespace Hypercube.Shared.EventBus;

public interface IEventBus
{
    void UnsubscribeEvent<T>(IEventSubscriber subscriber);
    public void SubscribeEvent<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : notnull;
    void RaiseEvent(object toRaise);
    void RaiseEvent<T>(ref T toRaise) where T : notnull;
    void RaiseEvent<T>(T toRaise) where T : notnull;
}
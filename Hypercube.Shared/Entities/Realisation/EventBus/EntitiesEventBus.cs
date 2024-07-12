using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Entities.Realisation.EventBus;

public sealed class EntitiesEventBus : IEntitiesEventBus
{
    public void SubscribeEvent<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : notnull
    {
        throw new NotImplementedException();
    }

    public void RaiseEvent(object toRaise)
    {
        throw new NotImplementedException();
    }

    public void RaiseEvent<T>(ref T toRaise) where T : notnull
    {
        throw new NotImplementedException();
    }

    public void RaiseEvent<T>(T toRaise) where T : notnull
    {
        throw new NotImplementedException();
    }
}
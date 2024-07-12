using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Entities.Realisation.EventBus;

public sealed class EntitiesEventBus : IEntitiesEventBus
{
    public void Unsubscribe<T>(IEventSubscriber subscriber)
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : notnull
    {
        throw new NotImplementedException();
    }

    public void Raise(object receiver)
    {
        throw new NotImplementedException();
    }

    public void Raise<T>(ref T receiver) where T : notnull
    {
        throw new NotImplementedException();
    }

    public void Raise<T>(T receiver) where T : notnull
    {
        throw new NotImplementedException();
    }
}
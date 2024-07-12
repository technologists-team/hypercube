using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Entities.Realisation.EventBus;

public sealed class EntitiesEventBus : IEntitiesEventBus
{
    public void Unsubscribe<T>(IEventSubscriber subscriber) where T : IEventArgs
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : IEventArgs
    {
        throw new NotImplementedException();
    }

    public void Raise(object receiver)
    {
        throw new NotImplementedException();
    }

    public void Raise<T>(ref T receiver) where T : IEventArgs
    {
        throw new NotImplementedException();
    }

    public void Raise<T>(T receiver) where T : IEventArgs
    {
        throw new NotImplementedException();
    }
}
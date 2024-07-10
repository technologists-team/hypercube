namespace Hypercube.Shared.Entities.Realisation.EventBus;

public sealed class EntitiesEventBus : IEntitiesEventBus
{
    public void Subscribe<T>(Action<T> callback)
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        throw new NotImplementedException();
    }

    public void Invoke<T>(T signal)
    {
        throw new NotImplementedException();
    }
}
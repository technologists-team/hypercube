namespace Hypercube.Shared.Entities.Realisation.EventBus;

public class EntitiesEventSubscription : IEquatable<EntitiesEventSubscription>
{
    public EntitiesEventRefHandler Handler { get; }
    private object Equality { get; }

    public EntitiesEventSubscription(EntitiesEventRefHandler handler, object equality)
    {
        Handler = handler;
        Equality = equality;
    }

    public bool Equals(EntitiesEventSubscription? other)
    {
        return other is not null && Equals(other.Equality, Equality);
    }

    public override bool Equals(object? obj)
    {
        return obj is EntitiesEventSubscription registration && Equals(registration);
    }

    public override int GetHashCode()
    {
        return Equality.GetHashCode();
    }
}
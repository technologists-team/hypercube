namespace Hypercube.Core.Ecs.Core.Events;

[EngineInternal]
public sealed class GlobalEventSubscription : IEquatable<GlobalEventSubscription>
{
    public GlobalEventRefHandler Handler { get; }
    public object Equality { get; }

    public GlobalEventSubscription(GlobalEventRefHandler handler, object equality)
    {
        Handler = handler;
        Equality = equality;
    }

    public bool Equals(GlobalEventSubscription? other)
    {
        return other is not null && Equals(other.Equality, Equality);
    }

    public override bool Equals(object? obj)
    {
        return obj is GlobalEventSubscription registration && Equals(registration);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Handler, Equality);
    }
}
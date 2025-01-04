namespace Hypercube.Core.IO.Prototypes;

public readonly struct PrototypeId<T> where T : IPrototype
{
    public readonly string Id;

    public PrototypeId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Prototype ID cannot be null or empty.", nameof(id));

        Id = id;
    }

    public override string ToString()
    {
        return Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is PrototypeId<T> other && string.Equals(Id, other.Id, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static implicit operator string(PrototypeId<T> prototypeId)
    {
        return prototypeId.Id;
    }

    public static explicit operator PrototypeId<T>(string id)
    {
        return new PrototypeId<T>(id);
    }


    public static bool operator ==(PrototypeId<T> left, PrototypeId<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PrototypeId<T> left, PrototypeId<T> right)
    {
        return !(left == right);
    }
}
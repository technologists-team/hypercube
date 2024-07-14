namespace Hypercube.Client.Audio;

public readonly struct AudioId(int value) : IEquatable<AudioId>
{
    public static readonly AudioId Invalid = new(-1);
    public static readonly AudioId Zero = new(0);

    private readonly int _value = value;
    
    public bool Equals(AudioId other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is AudioId id && Equals(id);
    }
    
    public static bool operator ==(AudioId a, AudioId b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(AudioId a, AudioId b)
    {
        return !a.Equals(b);
    }
    
    public static implicit operator int(AudioId id)
    {
        return id._value;
    }
    
    public override int GetHashCode()
    {
        return _value;
    }
    
    public override string ToString()
    {
        return $"audio({_value})";
    }
}
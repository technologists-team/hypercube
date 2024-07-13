namespace Hypercube.Client.Audio;

public readonly struct AudioId(int value) : IEquatable<AudioId>
{
    public static readonly AudioId Invalid = new(-1);
    public static readonly AudioId Zero = new(0);
    
    public readonly int Value = value;
    
    public bool Equals(AudioId other)
    {
        return Value == other.Value;
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
        return id.Value;
    }
    
    public override int GetHashCode()
    {
        return Value;
    }
    
    public override string ToString()
    {
        return $"audio({Value})";
    }
}
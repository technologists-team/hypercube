namespace Hypercube.Graphics.Monitors;

[Serializable]
public readonly struct MonitorId(int value) : IEquatable<MonitorId>
{
    public static readonly MonitorId Invalid = default;
    public static readonly MonitorId Zero = new(0);
    
    public readonly int Value = value;
    
    public bool Equals(MonitorId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is MonitorId id && Equals(id);
    }
    
    public static bool operator ==(MonitorId a, MonitorId b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(MonitorId a, MonitorId b)
    {
        return !a.Equals(b);
    }
    
    public static implicit operator int(MonitorId id)
    {
        return id.Value;
    }
    
    public static implicit operator MonitorId(int id)
    {
        return new MonitorId(id);
    }
    
    public override int GetHashCode()
    {
        return Value;
    }
    
    public override string ToString()
    {
        return $"monitor({Value})";
    }
}
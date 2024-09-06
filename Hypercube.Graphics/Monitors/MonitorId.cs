using JetBrains.Annotations;

namespace Hypercube.Graphics.Monitors;

/// <summary>
/// Registered monitor index. Wrapper over <see cref="int"/>,
/// to simplify typing and work with indexing.
/// </summary>
[PublicAPI, Serializable]
public readonly struct MonitorId : IEquatable<MonitorId>
{
    /// <summary>
    /// Incorrect monitor index.
    /// </summary>
    /// <value>
    /// MonitorId (-1)
    /// </value>
    public static readonly MonitorId Invalid = new(-1);
    
    /// <summary>
    /// Index from which monitors indexing start.
    /// </summary>
    /// <value>
    /// MonitorId (0)
    /// </value>
    public static readonly MonitorId Zero = new(0);
    
    public readonly int Value;

    public MonitorId(int value)
    {
        Value = value;
    }

    public bool Equals(MonitorId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is MonitorId id && Equals(id);
    }
    
    public override int GetHashCode()
    {
        return Value;
    }
    
    public override string ToString()
    {
        return $"monitor({Value})";
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
}
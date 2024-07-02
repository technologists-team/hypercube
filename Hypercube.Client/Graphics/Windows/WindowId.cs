namespace Hypercube.Client.Graphics.Windows;

[Serializable]
public readonly struct WindowId(int value) : IEquatable<WindowId>
{
    public static readonly WindowId Invalid = default;
    public static readonly WindowId Zero = new(0);
    
    internal readonly int Value = value;
    
    public bool Equals(WindowId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is WindowId id && Equals(id);
    }
    
    public static bool operator ==(WindowId a, WindowId b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(WindowId a, WindowId b)
    {
        return !a.Equals(b);
    }
    
    public static explicit operator int(WindowId id)
    {
        return id.Value;
    }
    
    public override int GetHashCode()
    {
        return Value;
    }
    
    public override string ToString()
    {
        return $"window({Value})";
    }
}
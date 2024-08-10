using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

[PublicAPI, Serializable]
public readonly struct WindowId : IEquatable<WindowId>
{
    public static readonly WindowId Invalid;
    public static readonly WindowId Zero = new(0);
    
    public readonly int Value;

    public WindowId(int value)
    {
        Value = value;
    }
    
    public bool Equals(WindowId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is WindowId id && Equals(id);
    }
    
    public override int GetHashCode()
    {
        return Value;
    }
    
    public override string ToString()
    {
        return $"window({Value})";
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
}
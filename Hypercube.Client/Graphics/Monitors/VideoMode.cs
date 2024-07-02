namespace Hypercube.Client.Graphics.Monitors;

public readonly struct VideoMode
{
    public ushort Width { get; init; }
    public ushort Height { get; init; }
    
    public byte RedBits { get; init; }
    public byte BlueBits { get; init; }
    public byte GreenBits { get; init; }
    
    public ushort RefreshRate { get; init; }

    public override string ToString()
    {
        return $"mode(size: {Width}, {Height}, RGB bits: {RedBits}, {BlueBits}, {GreenBits}, rate: {RefreshRate})";
    }
}
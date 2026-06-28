using System.Collections.Immutable;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Windowing.Windows;

public readonly struct Icon
{
    private readonly ImmutableArray<byte> _data;

    public ReadOnlySpan<byte> Data => _data.AsSpan();
    public Vector2i Size { get; }
    public int Channels { get; }

    public Icon(byte[] data, Vector2i size, int channels)
    {
        _data = [..data];
        
        Size = size;
        Channels = channels;
    }
}

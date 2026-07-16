using System.Collections.Immutable;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Windowing.Windows;

/// <summary>
/// Represents an image used as a window icon, containing raw pixel data and metadata.
/// </summary>
public readonly struct Icon
{
    private readonly ImmutableArray<byte> _data;

    /// <summary>
    /// Gets the dimensions of the icon in pixels.
    /// </summary>
    public Vector2i Size { get; }
     
    /// <summary>
    /// Gets the number of color channels per pixel (e.g., 4 for RGBA).
    /// </summary>
    public int Channels { get; }

    /// <summary>
    /// Gets the raw pixel data of the icon as a read-only span.
    /// </summary>
    public ReadOnlySpan<byte> Data => _data.AsSpan();

    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> struct.
    /// </summary>
    /// <param name="data">The raw pixel data array.</param>
    /// <param name="size">The dimensions of the icon in pixels.</param>
    /// <param name="channels">The number of color channels per pixel.</param>
    public Icon(byte[] data, Vector2i size, int channels)
    {
        _data = [..data];
        
        Size = size;
        Channels = channels;
    }
}

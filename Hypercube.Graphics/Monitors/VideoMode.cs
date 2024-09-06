using System.Diagnostics;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Monitors;

/// <summary>
/// Monitor mode information obtained through the graphics API.
/// </summary>
[PublicAPI, DebuggerDisplay("{ToString()}")]
public readonly struct VideoMode
{
    /// <summary>
    /// Width in screen coordinates.
    /// </summary>
    public ushort Width { get; init; }
    
    /// <summary>
    /// Height in screen coordinates.
    /// </summary>
    public ushort Height { get; init; }
    
    public byte RedBits { get; init; }
    public byte BlueBits { get; init; }
    public byte GreenBits { get; init; }
    
    public ushort RefreshRate { get; init; }

    public override string ToString()
    {
        return $"size: {Width}, {Height}; RGB bits: {RedBits}, {BlueBits}, {GreenBits}; rate: {RefreshRate}";
    }
}
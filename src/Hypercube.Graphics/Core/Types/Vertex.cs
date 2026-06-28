using System.Runtime.InteropServices;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Core.Types;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Vertex
{
    public const int Size = 12 * sizeof(float);

    public readonly Vector3 Position;
    public readonly Vector4 Color;
    public readonly Vector2 UVCoords;
    public readonly Vector3 Normal;

    public Vertex(Vector3 position, Vector2 uvCoords, Color color, Vector3 normal)
    {
        Position = position;
        UVCoords = uvCoords;
        Color = color.Vec4;
        Normal = normal;
    }
    
    public Vertex(Vector2 position, Vector2 uvCoords, Color color)
    {
        Position = new Vector3(position);
        UVCoords = uvCoords;
        Color = color.Vec4;
        Normal = Vector3.Zero;
    }
}

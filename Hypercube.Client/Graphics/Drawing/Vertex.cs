using System.Runtime.InteropServices;
using Hypercube.Math;
using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Drawing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Vertex
{
    public const int Size = 9;
    
    private static readonly Color DefaultColor = Color.White;

    public readonly Vector3 Position;
    public readonly Color Color;
    public readonly Vector2 UVCoords;
    
    public Vertex(Vector3 position, Vector2 uvCoords, Color color)
    {
        Position = position;
        UVCoords = uvCoords;
        Color = color;
    }
    
    public Vertex(Vector2 position, Vector2 uvCoords, Color color)
    {
        Position = new Vector3(position, 0.0f);
        UVCoords = uvCoords;
        Color = color;
    }

    public Vertex(Vector3 position, Vector2 uvCoords)
    {
        Position = position;
        UVCoords = uvCoords;
        Color = DefaultColor;
    }

    public float[] ToVertices()
    {
        return new[]
        {
            Position.X,
            Position.Y,
            Position.Z,
            Color.R,
            Color.G,
            Color.B,
            Color.A,
            UVCoords.X,
            UVCoords.Y
        };
    }
}
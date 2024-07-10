using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Vector;

public readonly struct Vector4(float x, float y, float z, float w)
{
    public readonly float X = x;
    public readonly float Y = y;
    public readonly float Z = z;
    public readonly float W = w;

    public Vector4(float value) : this(value, value, value, value)
    {
    }

    public Vector4(Vector2 vector2, float z, float w) : this(vector2.X, vector2.Y, z, w)
    {
    }

    public Vector4(Vector3 vector3, float w) : this(vector3.X, vector3.Y, vector3.Z, w)
    {
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{x}, {y}, {z}, {w}";
    }
}
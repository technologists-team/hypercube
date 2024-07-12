using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Quaternion : IEquatable<Quaternion> 
{
    public readonly Vector4 Vector;

    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.LengthSquared;
    }
    
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Length;
    }
    
    public Quaternion Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new (Vector.Normalized);
    }
    
    public Vector3 Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.XYZ;
    }
    
    public Angle Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector.W);
    }
    
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.X;
    }
    
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Y;
    }
    
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Z;
    }
    
    public float W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.W;
    }
    
    public Quaternion(Vector4 vector)
    {
        Vector = vector;
    }
    
    public Quaternion(Quaternion quaternion) : this(quaternion.Vector)
    {
    }
    
    public Quaternion(float x, float y, float z, float w) : this(new Vector4(x, y, z, w))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithX(float value)
    {
        return new Quaternion(Vector.WithX(value));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithY(float value)
    {
        return new Quaternion(Vector.WithY(value));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithZ(float value)
    {
        return new Quaternion(Vector.WithZ(value));
    }    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithW(float value)
    {
        return new Quaternion(Vector.WithW(value));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Quaternion other)
    {
        return Vector == other.Vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Quaternion other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Vector.GetHashCode();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(a.Vector * b.Vector);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator *(Quaternion a, float b)
    {
        return new Quaternion(a.Vector * b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quaternion a, Quaternion b)
    {
        return a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quaternion a, Quaternion b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion FromEuler(Vector3 vector3)
    {
        throw new NotImplementedException();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ToEuler(Quaternion quaternion)
    {
        throw new NotImplementedException();
    }
}
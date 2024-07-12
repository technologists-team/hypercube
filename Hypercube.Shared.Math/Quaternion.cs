using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Quaternion : IEquatable<Quaternion> 
{
    private const float SingularityThreshold = 0.4999995f;
    
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
    public Vector3 ToEuler()
    {
        return ToEuler(this);
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

    /// <summary>
    /// Convert this instance to an Euler angle representation.
    /// <remarks>
    /// Taken from <a href="https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/Data/Quaterniond.cs#L190">OpenTK.Mathematics/Data/Quaterniond.cs</a>
    /// </remarks>
    /// </summary>
    /// <returns>Euler angle in radians</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ToEuler(Quaternion quaternion)
    {
        var sqx = quaternion.X * quaternion.X;
        var sqy = quaternion.Y * quaternion.Y;
        var sqz = quaternion.Z * quaternion.Z;
        var sqw = quaternion.W * quaternion.W;
        
        var unit = sqx + sqy + sqz + sqw; // If normalised is one, otherwise is correction factor
        var singularityTest = quaternion.X * quaternion.Z + quaternion.W * quaternion.Y;
        
        if (singularityTest > SingularityThreshold * unit)
            // Singularity at north pole
            return new Vector3(
                0,
                HyperMath.PiOver2,
                2f * MathF.Atan2(quaternion.X, quaternion.W)
            );

        if (singularityTest < -SingularityThreshold * unit)
            // Singularity at south pole
            return new Vector3(
                0,
                -HyperMath.PiOver2,
                -2f * MathF.Atan2(quaternion.X, quaternion.W)
            );

        return new Vector3(
            MathF.Atan2(2 * (quaternion.W * quaternion.X - quaternion.Y * quaternion.Z), sqw - sqx - sqy + sqz),
            MathF.Asin(2 * singularityTest / unit),
            MathF.Atan2(2 * (quaternion.W * quaternion.Z - quaternion.X * quaternion.Y), sqw + sqx - sqy - sqz)
        );
    }
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Math.Vector;

namespace Hypercube.Math;

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
    
    public Quaternion(Vector3 vector3) : this(FromEuler(vector3).Vector)
    {
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
    public override string ToString()
    {
        return Vector.ToString();
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
    public static Quaternion FromEuler(float x, float y, float z)
    {
        return FromEuler(new Vector3(x, y, z));
    }

    /// <summary>
    /// Created new <see cref="Quaternion"/> from given Euler angles in radians.
    /// <remarks>
    /// Taken from <a href="https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/Data/Quaternion.cs#L77">OpenTK.Mathematics/Data/Quaternion.cs</a>
    /// </remarks>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion FromEuler(Vector3 vector3)
    {
        var axis = vector3 / 2f;
        
        var c1 = MathF.Cos(axis.X);
        var c2 = MathF.Cos(axis.Y);
        var c3 = MathF.Cos(axis.Z);
        
        var s1 = MathF.Sin(axis.X);
        var s2 = MathF.Sin(axis.Y);
        var s3 = MathF.Sin(axis.Z);

        return new Quaternion(
            s1 * c2 * c3 + c1 * s2 * s3,
            c1 * s2 * c3 - s1 * c2 * s3,
            c1 * c2 * s3 + s1 * s2 * c3,
            c1 * c2 * c3 - s1 * s2 * s3
        );
    } 

    /// <summary>
    /// Convert this instance to an Euler angle representation.
    /// <remarks>
    /// Taken from <a href="https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/Data/Quaternion.cs#L194">OpenTK.Mathematics/Data/Quaternion.cs</a>
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
                HyperMathF.PIOver2,
                2f * MathF.Atan2(quaternion.X, quaternion.W)
            );

        if (singularityTest < -SingularityThreshold * unit)
            // Singularity at south pole
            return new Vector3(
                0,
                -HyperMathF.PIOver2,
                -2f * MathF.Atan2(quaternion.X, quaternion.W)
            );

        return new Vector3(
            MathF.Atan2(2 * (quaternion.W * quaternion.X - quaternion.Y * quaternion.Z), sqw - sqx - sqy + sqz),
            MathF.Asin(2 * singularityTest / unit),
            MathF.Atan2(2 * (quaternion.W * quaternion.Z - quaternion.X * quaternion.Y), sqw + sqx - sqy - sqz)
        );
    }
}
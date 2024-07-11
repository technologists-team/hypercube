using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math;

public readonly struct Angle(double theta)
{
    public static readonly Angle Zero = new(0);

    public readonly double Theta = theta;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Angle angle)
    {
        return (float)angle.Theta;
    }
}
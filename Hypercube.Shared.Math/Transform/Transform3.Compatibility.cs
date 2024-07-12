using System.Runtime.CompilerServices;

namespace Hypercube.Shared.Math.Transform;

public partial struct Transform3
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Transform2(Transform3 transform3)
    {
        return new Transform2(transform3.Position, new Angle(transform3.Rotation.ToEuler().Z), transform3.Scale);
    }
}
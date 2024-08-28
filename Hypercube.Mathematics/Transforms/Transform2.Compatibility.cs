using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.Transforms;

public partial struct Transform2
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Transform3(Transform2 transform2)
    {
        return new Transform3(transform2.Position, new Quaternion(Vector3.UnitZ * (float)transform2.Rotation), transform2.Scale);
    }
}
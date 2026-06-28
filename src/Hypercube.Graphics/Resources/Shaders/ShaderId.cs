using Hypercube.Utilities.Attributes;

namespace Hypercube.Graphics.Resources.Shaders;

[IdStruct(typeof(int))]
public readonly partial struct ShaderId
{
    public static readonly ShaderId Null = -1;
}

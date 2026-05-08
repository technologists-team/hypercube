using Hypercube.Utilities.Attributes;

namespace Hypercube.Core.Audio;

[IdStruct(typeof(uint))]
public readonly partial struct AudioHandle
{
    public static readonly AudioHandle Zero = new(0);
}

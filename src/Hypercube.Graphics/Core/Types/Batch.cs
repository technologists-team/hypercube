using Hypercube.Graphics.Resources;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Utilities.Collections;

namespace Hypercube.Graphics.Core.Types;

public struct Batch
{
    public FixedArray4<TextureId> Textures;
    public BlendMode BlendMode;
    public PrimitiveType PrimitiveType;
    public CullFaceMode CullFaceMode;
}

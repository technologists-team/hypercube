using Hypercube.Utilities.Attributes;

namespace Hypercube.Graphics.Resources.Textures;

[IdStruct(typeof(int))]
public readonly partial struct TextureId
{
    public static readonly TextureId Null = -1;
}

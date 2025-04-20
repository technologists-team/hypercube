using System.Collections.ObjectModel;
using Hypercube.Graphics.Texturing;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources.Loaders;

namespace Hypercube.Graphics;

public sealed class Texture : Resource, IImage
{
    public Vector2i Size { get; }
    public ReadOnlyCollection<byte> Data { get; }
    public Box2 UV { get; }

    public Texture(Vector2i size, byte[] data, Box2 uv)
    {
        Size = size;
        Data = data.AsReadOnly();
        UV = uv;
    }

    public override void Dispose()
    {
        
    }
}
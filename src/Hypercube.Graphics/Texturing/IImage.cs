using System.Collections.ObjectModel;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Texturing;

public interface IImage
{
    Vector2i Size { get; }
    ReadOnlyCollection<byte> Data { get; }
    Box2 UV { get; }

    int Width => Size.X;
    int Height => Size.Y;
}
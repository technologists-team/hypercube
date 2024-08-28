using Hypercube.Mathematics.Shapes;
using Hypercube.Resources;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing;

[PublicAPI]
public interface ITexture
{
    ResourcePath Path { get; }
    int Width { get; }
    int Height { get; }
    byte[] Data { get; }
    
    Box2 Quad { get; }
}
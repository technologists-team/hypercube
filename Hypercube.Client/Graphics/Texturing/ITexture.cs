using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Client.Graphics.Texturing;

public interface ITexture
{
    int Width { get; }
    int Height { get; }
    byte[] Data { get; }
    Box2 QuadCrateTranslated(Vector2 position);
}
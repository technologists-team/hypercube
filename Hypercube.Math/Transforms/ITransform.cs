using Hypercube.Math.Matrices;

namespace Hypercube.Math.Transforms;

public interface ITransform
{
    Matrix4X4 Matrix { get; }
}
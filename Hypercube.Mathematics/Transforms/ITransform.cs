using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.Transforms;

public interface ITransform
{
    Matrix4X4 Matrix { get; }
}
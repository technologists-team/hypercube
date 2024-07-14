using Hypercube.Math.Matrixs;

namespace Hypercube.Math.Transforms;

public interface ITransform
{
    Matrix4X4 Matrix { get; }
}
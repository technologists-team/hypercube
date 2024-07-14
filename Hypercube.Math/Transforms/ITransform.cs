using Hypercube.Math.Matrix;

namespace Hypercube.Math.Transform;

public interface ITransform
{
    Matrix4X4 Matrix { get; }
}
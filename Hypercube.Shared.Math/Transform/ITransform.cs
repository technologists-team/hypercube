using Hypercube.Shared.Math.Matrix;

namespace Hypercube.Shared.Math.Transform;

public interface ITransform
{
    Matrix4X4 Matrix { get; }
}
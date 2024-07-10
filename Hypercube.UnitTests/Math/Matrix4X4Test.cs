using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.UnitTests.Math;

public sealed class Matrix4X4Test
{
    [Test]
    public void Equals()
    {
        var matrixA = new Matrix4X4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        var matrixB = new Matrix4X4(16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
        
        // ReSharper disable once EqualExpressionComparison
        Assert.That(matrixA == matrixA);
        Assert.That(matrixA.Equals(matrixA));
        Assert.That(matrixA.Equals((object)matrixA));

        var matrixAClone = new Matrix4X4(matrixA);
        Assert.That(matrixA == matrixAClone);
        Assert.That(matrixA.Equals(matrixAClone));
        Assert.That(matrixA.Equals((object)matrixAClone));
        
        Assert.That(matrixA != matrixB);
        Assert.That(!matrixA.Equals(matrixB));
        Assert.That(!matrixA.Equals((object)matrixB));

        Assert.Pass($"{nameof(Matrix4X4)} equals passed");
    }
    
    [Test]
    public void Multiplication()
    {
        var matrixA = new Matrix4X4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        var matrixB = new Matrix4X4(16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
        var vectorA = new Vector4(20, 60, 32, 7);
        
        Assert.That(matrixA * matrixB == new Matrix4X4(80, 70, 60, 50, 240, 214, 188, 162, 400, 358, 316, 274, 560, 502, 444, 386));
        Assert.That(matrixA * vectorA == new Vector4(264, 740, 1216, 1692));
        Assert.That(matrixB * vectorA == new Vector4(1759, 1283, 807, 331));
        
        Assert.Pass($"{nameof(Matrix4X4)} multiplication passed");
    }
}
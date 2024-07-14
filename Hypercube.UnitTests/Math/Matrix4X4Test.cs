using Hypercube.Math.Matrix;
using Hypercube.Math.Vector;

namespace Hypercube.UnitTests.Math;

public static class Matrix4X4Test
{
    [Test]
    public static void Multiplication()
    {
        var matrixA = new Matrix4X4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        var matrixB = new Matrix4X4(16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
        var vectorA = new Vector4(20, 60, 32, 7);
        
        Assert.Multiple(() =>
        {
            Assert.That(matrixA * matrixB, Is.EqualTo(new Matrix4X4(80, 70, 60, 50, 240, 214, 188, 162, 400, 358, 316, 274, 560, 502, 444, 386)));
            Assert.That(matrixA * vectorA, Is.EqualTo(new Vector4(264, 740, 1216, 1692)));
            Assert.That(matrixB * vectorA, Is.EqualTo(new Vector4(1759, 1283, 807, 331)));
        });
        
        Assert.Pass($"{nameof(Matrix4X4)} multiplication passed");
    }
}
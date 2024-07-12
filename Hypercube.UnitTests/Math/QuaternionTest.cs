using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.UnitTests.Math;

public class QuaternionTest
{
    [Test]
    public void Equals()
    {
        var quaternionA = new Quaternion(1, 2, 3, 4);
        var quaternionB = new Quaternion(10, 22, 40, 32);
        
        // ReSharper disable once EqualExpressionComparison
        Assert.That(quaternionA == quaternionA);
        Assert.That(quaternionA.Equals(quaternionA));
        Assert.That(quaternionA.Equals((object)quaternionA));

        var quaternionAClone = new Quaternion(quaternionA);
        Assert.That(quaternionA == quaternionAClone);
        Assert.That(quaternionA.Equals(quaternionAClone));
        Assert.That(quaternionA.Equals((object)quaternionAClone));
        
        Assert.That(quaternionA != quaternionB);
        Assert.That(!quaternionA.Equals(quaternionB));
        Assert.That(!quaternionA.Equals((object)quaternionB));

        Assert.Pass($"{nameof(Quaternion)} equals passed");
    }

    /// <summary>
    /// Can ba useful, cite to 3d convert: https://www.andre-gaschler.com/rotationconverter/
    /// </summary>
    [Test]
    public void ToEuler()
    {
        var eulerA = new Quaternion(1, 2, 3, 4).ToEuler();
        var eulerB = new Quaternion(0, 0.6767456f, 0.4308296f, 0.5969936f).ToEuler();
        
        Assert.That(eulerA == new Vector3(-0.19739556f, 0.8232120f, 1.3734008f));
        Assert.That(eulerB == new Vector3(-1.42767680f, 0.9407929f, 2.0799970f));
        
        Assert.Pass($"{nameof(Quaternion)} to euler passed");
    }

    [Test]
    public void FromEulerUnit()
    {
        var quaternionUnitX = Quaternion.FromEuler(Vector3.UnitX);
        var quaternionUnitY = Quaternion.FromEuler(Vector3.UnitY);
        var quaternionUnitZ = Quaternion.FromEuler(Vector3.UnitZ);

        Assert.That(quaternionUnitX == new Quaternion(0.47942555f, 0, 0, 0.87758255f));
        Assert.That(quaternionUnitY == new Quaternion(0, 0.47942555f, 0, 0.87758255f));
        Assert.That(quaternionUnitZ == new Quaternion(0, 0, 0.47942555f, 0.87758255f));
        
        Assert.Pass($"{nameof(Quaternion)} from euler unit passed");
    }

    [Test]
    public void FromEulerConvert()
    {
        var quaternionA = new Quaternion(0.8232120f, 0.6767456f, 0.4308296f, 0.5969936f);
        var eulerA = quaternionA.ToEuler();
        var fromA = Quaternion.FromEuler(eulerA);
        
        // The losses on this convert are fucked up.
        Assert.That(fromA == new Quaternion(0.6355612f, 0.5224818f, 0.33262214f, 0.4609092f));
        
        Assert.Pass($"{nameof(Quaternion)} from euler convert passed");
    }
}
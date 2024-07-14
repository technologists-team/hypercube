using Hypercube.Math;
using Hypercube.Math.Vector;

namespace Hypercube.UnitTests.Math;

public static class QuaternionTest
{
    /// <summary>
    /// Can ba useful, cite to 3d convert: https://www.andre-gaschler.com/rotationconverter/
    /// </summary>
    [Test]
    public static void ToEuler()
    {
        var eulerA = new Quaternion(1, 2, 3, 4).ToEuler();
        var eulerB = new Quaternion(0, 0.6767456f, 0.4308296f, 0.5969936f).ToEuler();
        
        Assert.Multiple(() =>
        {
            Assert.That(eulerA, Is.EqualTo(new Vector3(-0.19739556f, 0.8232120f, 1.3734008f)));
            Assert.That(eulerB, Is.EqualTo(new Vector3(-1.42767680f, 0.9407929f, 2.0799970f)));
        });
        
        Assert.Pass($"{nameof(Quaternion)} to euler passed");
    }

    [Test]
    public static void FromEulerUnit()
    {
        var quaternionUnitX = Quaternion.FromEuler(Vector3.UnitX);
        var quaternionUnitY = Quaternion.FromEuler(Vector3.UnitY);
        var quaternionUnitZ = Quaternion.FromEuler(Vector3.UnitZ);
        
        Assert.Multiple(() =>
        {
            Assert.That(quaternionUnitX, Is.EqualTo(new Quaternion(0.47942555f, 0, 0, 0.87758255f)));
            Assert.That(quaternionUnitY, Is.EqualTo(new Quaternion(0, 0.47942555f, 0, 0.87758255f)));
            Assert.That(quaternionUnitZ, Is.EqualTo(new Quaternion(0, 0, 0.47942555f, 0.87758255f)));
        });
        
        Assert.Pass($"{nameof(Quaternion)} from euler unit passed");
    }

    [Test]
    public static void FromEulerConvert()
    {
        var quaternionA = new Quaternion(0.8232120f, 0.6767456f, 0.4308296f, 0.5969936f);
        var eulerA = quaternionA.ToEuler();
        var fromA = Quaternion.FromEuler(eulerA);
        
        // The losses on this convert are fucked up
        Assert.That(fromA, Is.EqualTo(new Quaternion(0.6355612f, 0.5224818f, 0.33262214f, 0.4609092f)));
        
        Assert.Pass($"{nameof(Quaternion)} from euler convert passed");
    }
}
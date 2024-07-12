using Hypercube.Shared.Math;

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
}
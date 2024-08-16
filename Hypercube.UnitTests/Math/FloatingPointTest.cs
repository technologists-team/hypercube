using Hypercube.Mathematics.Extensions;

namespace Hypercube.UnitTests.Math;

public static class FloatingPointTest
{
    [Test]
    public static void Equals()
    {
        Assert.Multiple(() =>
        {
            Assert.That((0.1d + 0.2d).AboutEquals(0.3d));
            Assert.That((0.1f + 0.2f).AboutEquals(0.3f));
        });
        
        Assert.Pass($"{nameof(FloatingPointEqualsExtension)} passed");
    }
}
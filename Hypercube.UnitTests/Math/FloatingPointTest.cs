using Hypercube.Shared.Math.Extensions;

namespace Hypercube.UnitTests.Math;

public class FloatingPointTest
{
    [Test]
    public void Equals()
    {
        Assert.That((0.1d + 0.2d).AboutEquals(0.3d));
        Assert.That((0.1f + 0.2f).AboutEquals(0.3f));
        
        Assert.Pass($"{nameof(FloatingPointEqualsExtension)} passed");
    }
}
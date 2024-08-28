using Hypercube.Mathematics.Vectors;

namespace Hypercube.UnitTests.Math;

[TestFixture]
public sealed class CompareToTest
{
    [Test]
    public void CompareTo_LengthComparison_ReturnsExpectedResult()
    {
        // Arrange
        var vector1 = new Vector2(3, 4); // Length = 5
        var vector2 = new Vector2(6, 8); // Length = 10

        // Act
        var result = vector1.CompareTo(vector2);

        // Assert
        Assert.That(result, Is.LessThan(0));
    }
}
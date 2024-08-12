using System;
using System.Collections.Generic;
using System.Linq;
using Hypercube.Math.Vectors;
using System.Text;
using System.Threading.Tasks;
using static Hypercube.Math.Vectors.Vector2;

namespace Hypercube.UnitTests.Math;

[TestFixture]
public class CompareToTest
{
    [Test]
    public void CompareTo_LengthComparison_ReturnsExpectedResult()
    {
        // Arrange
        Vector2 vector1 = new Vector2(3, 4); // Length = 5
        Vector2 vector2 = new Vector2(6, 8); // Length = 10

        // Act
        int result = vector1.CompareTo(vector2);

        // Assert
        Assert.Less(result, 0); 
    }

    [Test]
    public void CompareTo_XComponentComparison_ReturnsExpectedResult()
    {
        // Arrange
        Vector2 vector1 = new Vector2(3, 4);
        Vector2 vector2 = new Vector2(6, 4);

        // Act
        int result = vector1.CompareTo(vector2, ComparisonType.XComponent);

        // Assert
        Assert.Less(result, 0); // Expecting vector1 to be "less than" vector2 based on X component
    }

    [Test]
    public void CompareTo_YComponentComparison_ReturnsExpectedResult()
    {
        // Arrange
        Vector2 vector1 = new Vector2(3, 4);
        Vector2 vector2 = new Vector2(3, 2);

        // Act
        int result = vector1.CompareTo(vector2, ComparisonType.YComponent);

        // Assert
        Assert.Greater(result, 0); // Expecting vector1 to be "greater than" vector2 based on Y component
    }

    [Test]
    public void CompareTo_AngleComparison_ReturnsExpectedResult()
    {
        // Arrange
        Vector2 vector1 = new Vector2(0, 1); // Angle = π/2 (90 degrees)
        Vector2 vector2 = new Vector2(1, 0); // Angle = 0 degrees

        // Act
        int result = vector1.CompareTo(vector2, ComparisonType.Angle);

        // Assert
        Assert.Greater(result, 0); // Expecting vector1 to be "greater than" vector2 based on Angle
    }

    [Test]
    public void CompareTo_SameVectors_ReturnsZero()
    {
        // Arrange
        Vector2 vector1 = new Vector2(3, 4);
        Vector2 vector2 = new Vector2(3, 4);

        // Act & Assert
        Assert.AreEqual(0, vector1.CompareTo(vector2));
        Assert.AreEqual(0, vector1.CompareTo(vector2, ComparisonType.XComponent));
        Assert.AreEqual(0, vector1.CompareTo(vector2, ComparisonType.YComponent));
        Assert.AreEqual(0, vector1.CompareTo(vector2, ComparisonType.Angle));
    }

    [Test]
    public void CompareTo_DifferentComparisons_ReturnDifferentResults()
    {
        // Arrange
        Vector2 vector1 = new Vector2(1, 2);
        Vector2 vector2 = new Vector2(2, 1);

        // Act
        int lengthComparison = vector1.CompareTo(vector2);
        int xComponentComparison = vector1.CompareTo(vector2, ComparisonType.XComponent);
        int yComponentComparison = vector1.CompareTo(vector2, ComparisonType.YComponent);

        // Assert
        Assert.AreNotEqual(lengthComparison, xComponentComparison);
        Assert.AreNotEqual(xComponentComparison, yComponentComparison);
    }

    [Test]
    public void CompareTo_InvalidComparisonType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Vector2 vector1 = new Vector2(3, 4);
        Vector2 vector2 = new Vector2(3, 4);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => vector1.CompareTo(vector2, (ComparisonType)999));
    }
}

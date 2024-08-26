using System;
using Hypercube.Math.Vectors;
using NUnit.Framework;

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
        Assert.Less(result, 0);
    }

    [Test]
    public void CompareTo_XComponentComparison_ReturnsExpectedResult()
    {
        // Arrange
        var vector1 = new Vector2(3, 4);
        var vector2 = new Vector2(6, 4);

        // Act
        var result = vector1.CompareTo(vector2, v => v.X);

        // Assert
        Assert.Less(result, 0); // Expecting vector1 to be "less than" vector2 based on X component
    }

    [Test]
    public void CompareTo_YComponentComparison_ReturnsExpectedResult()
    {
        // Arrange
        var vector1 = new Vector2(3, 4);
        var vector2 = new Vector2(3, 2);

        // Act
        var result = vector1.CompareTo(vector2, v => v.Y);

        // Assert
        Assert.Greater(result, 0); // Expecting vector1 to be "greater than" vector2 based on Y component
    }

    [Test]
    public void CompareTo_AngleComparison_ReturnsExpectedResult()
    {
        // Arrange
        var vector1 = new Vector2(0, 1); // Angle = π/2 (90 degrees)
        var vector2 = new Vector2(1, 0); // Angle = 0 degrees

        // Act
        var result = vector1.CompareTo(vector2, v => v.Angle);

        // Assert
        Assert.Greater(result, 0); // Expecting vector1 to be "greater than" vector2 based on Angle
    }

    [Test]
    public void CompareTo_SameVectors_ReturnsZero()
    {
        // Arrange
        var vector1 = new Vector2(3, 4);
        var vector2 = new Vector2(3, 4);

        // Act & Assert
        Assert.AreEqual(0, vector1.CompareTo(vector2));
        Assert.AreEqual(0, vector1.CompareTo(vector2, v => v.X));
        Assert.AreEqual(0, vector1.CompareTo(vector2, v => v.Y));
        Assert.AreEqual(0, vector1.CompareTo(vector2, v => v.Angle));
    }

    [Test]
    public void CompareTo_DifferentComparisons_ReturnDifferentResults()
    {
        // Arrange
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(2, 1);

        // Act
        var lengthComparison = vector1.CompareTo(vector2);
        var xComponentComparison = vector1.CompareTo(vector2, v => v.X);
        var yComponentComparison = vector1.CompareTo(vector2, v => v.Y);

        // Assert
        Assert.AreNotEqual(lengthComparison, xComponentComparison);
        Assert.AreNotEqual(xComponentComparison, yComponentComparison);
    }
}
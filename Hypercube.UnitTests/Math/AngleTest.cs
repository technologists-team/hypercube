using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.UnitTests.Math;

public static class AngleTest
{
    [Test]
    public static void Degrees()
    {
        Assert.Multiple(() =>
        {
            Assert.That(new Angle(HyperMath.PI).Degrees, Is.EqualTo(180d).Within(0.01d));
            Assert.That(new Angle(HyperMath.PIOver2).Degrees, Is.EqualTo(90d).Within(0.01d));
            Assert.That(new Angle(HyperMath.PIOver4).Degrees, Is.EqualTo(45d).Within(0.01d));
            Assert.That(new Angle(HyperMath.PIOver6).Degrees, Is.EqualTo(30d).Within(0.01d));

            Assert.That(Angle.FromDegrees(180d), Is.EqualTo(new Angle(HyperMath.PI)));
            Assert.That(Angle.FromDegrees(90d), Is.EqualTo(new Angle(HyperMath.PIOver2)));
            Assert.That(Angle.FromDegrees(45d), Is.EqualTo(new Angle(HyperMath.PIOver4)));
            Assert.That(Angle.FromDegrees(30d), Is.EqualTo(new Angle(HyperMath.PIOver6)));
        });
        
        Assert.Pass($"{nameof(Angle)} degrees passed");
    }

    [Test]
    public static void Vector()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Angle.Zero.GetRoundVector(), Is.EqualTo(Vector2.UnitX));
            Assert.That(new Angle(HyperMath.PI).GetRoundVector(), Is.EqualTo(-Vector2.UnitX));
            Assert.That(new Angle(-HyperMath.PI).GetRoundVector(), Is.EqualTo(-Vector2.UnitX));

            Assert.That(new Angle(HyperMath.PIOver2).GetRoundVector(), Is.EqualTo(Vector2.UnitY));
            Assert.That(new Angle(HyperMath.ThreePiOver2).GetRoundVector(), Is.EqualTo(-Vector2.UnitY));
            Assert.That(new Angle(-HyperMath.PIOver2).GetRoundVector(), Is.EqualTo(-Vector2.UnitY));

            Assert.That(new Angle(HyperMath.PIOver4).GetRoundVector(), Is.EqualTo(Vector2.One.Normalized));
            Assert.That(new Angle(HyperMath.ThreePiOver2 - HyperMath.PIOver4).GetRoundVector(), Is.EqualTo(-Vector2.One.Normalized));
        });
        
        Assert.Pass($"{nameof(Angle)} vector passed");
    }

    private static Vector2 GetRoundVector(this Angle angle)
    {
        var vector = angle.Vector;
        var x = MathF.Abs(vector.X) < 1e-15f ? 0 : vector.X;
        var y = MathF.Abs(vector.Y) < 1e-15f ? 0 : vector.Y;
        return new Vector2(x, y);
    }
}
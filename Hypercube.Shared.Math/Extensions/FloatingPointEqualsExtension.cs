namespace Hypercube.Shared.Math.Extensions;

public static class FloatingPointEqualsExtension
{
    public static bool AboutEquals(this double a, double b)
    {
        var epsilon = System.Math.Max(System.Math.Abs(a), System.Math.Abs(b)) * 1E-15d;
        return System.Math.Abs(a - b) <= epsilon;
    }
    
    public static bool AboutEquals(this float a, float b)
    {
        var epsilon = System.Math.Max(System.Math.Abs(a), System.Math.Abs(b)) * 1E-15f;
        return System.Math.Abs(a - b) <= epsilon;
    }
}
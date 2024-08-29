namespace Hypercube.Mathematics;

public static class HyperMath
{
    public const double PI = Math.PI;
    
    public const double PIOver2 = PI / 2;
    public const double PIOver4 = PI / 4;
    public const double PIOver6 = PI / 6;

    public const double TwoPI = 2 * PI;
    public const double ThreePiOver2 = 3 * PI / 2;

    public const double RadiansToDegrees = 180 / PI;
    public const double DegreesToRadians = PI / 180;
    
    public static int MoveTowards(int current, int target, int distance)
    {
        return current < target ?
            Math.Min(current + distance, target) :
            Math.Max(current - distance, target);
    }
}
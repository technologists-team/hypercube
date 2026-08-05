namespace Hypercube.Graphics;

// TODO: move Hypercube.Utilities.Unsafe
public static class Utilities
{
    public static unsafe void AsciiToPtr(string src, byte* dest, int max)
    {
        var i = 0;
        for (; i < src.Length && i < max - 1; i++)
        {
            var c = src[i];
            if (c > 127)
                throw new ArgumentException($"The character {c} is not ASCII (only characters 0–127 are allowed)");
            
            dest[i] = (byte) c;
        }

        dest[i] = 0;
    }
}
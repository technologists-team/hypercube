using System.Globalization;
using System.Runtime.InteropServices;
using Hypercube.Math.Vector;

namespace Hypercube.Math;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Color
{
    public static readonly Color White = new(1f, 1f, 1f);
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color Red = new(1f, 0, 0);
    public static readonly Color Green = new(0, 1f, 0);
    public static readonly Color Blue = new(0, 0, 1f);
    
    public byte ByteR => (byte)(R * byte.MaxValue);
    public byte ByteG => (byte)(G * byte.MaxValue);
    public byte ByteB => (byte)(B * byte.MaxValue);
    public byte ByteA => (byte)(A * byte.MaxValue);
    
    public readonly float R;
    public readonly float G;
    public readonly float B;
    public readonly float A;

    public Color(float r, float g, float b, float a = 1.0f)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
    public Color(byte r, byte g, byte b, byte a = byte.MaxValue)
    {
        R = (float)r / byte.MaxValue;
        G = (float)g / byte.MaxValue;
        B = (float)b / byte.MaxValue;
        A = (float)a / byte.MaxValue;
    }
    
    public Color(Vector3 vector3, float a = 1.0f)
    {
        R = vector3.X;
        G = vector3.Y;
        B = vector3.Z;
        A = a;
    }

    public Color(Vector4 vector4)
    {
        R = vector4.X;
        G = vector4.Y;
        B = vector4.Z;
        A = vector4.W;
    }
    
    
    public Color(Color color)
    {
        R = color.R;
        G = color.G;
        B = color.B;
        A = color.A;
    }
    
    public Color(string hex)
    {
        if (hex.StartsWith('#'))
            hex.TrimStart('#');

        if (hex.Length > 6)
        {
            R = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            G = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            B = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        }

        if (hex.Length != 8)
            throw new ArgumentException();
        
        A = int.Parse(hex.Substring(6, 4), NumberStyles.HexNumber);
    }

    public static Color FromHSV(float hue, float saturation, float value, float alpha = 1f)
    {
        return FromHSV(new Vector4(hue, saturation, value, alpha));
    }
    
    public static Color FromHSV(Vector4 hsv)
    {
        var hue = (hsv.X - MathF.Truncate(hsv.X)) * 360.0f;
        var saturation = hsv.Y;
        var value = hsv.Z;

        var c = value * saturation;

        var h = hue / 60.0f;
        var x = c * (1.0f - MathF.Abs(h % 2.0f - 1.0f));

        float r, g, b;
        if (0.0f <= h && h < 1.0f)
        {
            r = c;
            g = x;
            b = 0.0f;
        }
        else if (1.0f <= h && h < 2.0f)
        {
            r = x;
            g = c;
            b = 0.0f;
        }
        else if (2.0f <= h && h < 3.0f)
        {
            r = 0.0f;
            g = c;
            b = x;
        }
        else if (3.0f <= h && h < 4.0f)
        {
            r = 0.0f;
            g = x;
            b = c;
        }
        else if (4.0f <= h && h < 5.0f)
        {
            r = x;
            g = 0.0f;
            b = c;
        }
        else if (5.0f <= h && h < 6.0f)
        {
            r = c;
            g = 0.0f;
            b = x;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        var m = value - c;
        return new Color(r + m, g + m, b + m, hsv.W);
    }
}
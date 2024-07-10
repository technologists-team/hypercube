using System.Globalization;
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math;

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
}
using Hypercube.Shared.Math.Vector;

namespace Hypercube.Shared.Math.Matrix;

public readonly struct Matrix4X4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
{
    public readonly Vector4 X = x;
    public readonly Vector4 Y = y;
    public readonly Vector4 Z = z;
    public readonly Vector4 W = w;

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public float M00 => X.X;
    
    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public float M01 => X.Y;
    
    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public float M02 => X.Z;
    
    /// <summary>
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public float M03 => X.W;
    
    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public float M10 => Y.X;
    
    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public float M11 => Y.Y;
    
    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public float M12 => Y.Z;
    
    /// <summary>
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public float M13 => Y.W;
    
    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public float M20 => Z.X;
    
    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public float M21 => Z.Y;
    
    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public float M22 => Z.Z;
    
    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public float M23 => Z.W;
    
    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public float M30 => W.X;
    
    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public float M31 => W.Y;
    
    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public float M32 => W.Z;
    
    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public float M33 => W.W;
    
    public Matrix4X4(float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33) : this(new Vector4(m00, m01, m02, m03),
        new Vector4(m10, m11, m12, m13),
        new Vector4(m20, m21, m22, m23),
        new Vector4(m30, m31, m32, m33))
    {
    }

    public override string ToString()
    {
        return $"{X}\n{Y}\n{Z}\n{W}";
    }
}
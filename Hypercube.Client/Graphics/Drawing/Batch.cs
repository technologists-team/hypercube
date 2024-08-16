using Hypercube.Mathematics.Matrices;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Drawing;

public readonly struct Batch(int start, int size, int? textureHandle, PrimitiveType primitiveType, Matrix4X4 model)
{
    public readonly int Start = start;
    public readonly int Size = size;
    public readonly int? TextureHandle = textureHandle;
    public readonly PrimitiveType PrimitiveType = primitiveType;
    public readonly Matrix4X4 Model = model;
}
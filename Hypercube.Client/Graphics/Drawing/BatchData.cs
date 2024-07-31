using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Drawing;

public readonly struct BatchData
{
    public readonly int? TextureHandle;
    public readonly int ShaderHandle;
    public readonly PrimitiveType PrimitiveType;
    public readonly int StartIndex;

    public BatchData(int? textureHandle, int shaderHandle, PrimitiveType primitiveType, int startIndex)
    {
        TextureHandle = textureHandle;
        ShaderHandle = shaderHandle;
        PrimitiveType = primitiveType;
        StartIndex = startIndex;
    }
}
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Drawing;

public readonly record struct BatchData
{
    public readonly PrimitiveType PrimitiveType;
    public readonly int StartIndex;
    public readonly int? TextureHandle;
    public readonly int ShaderHandle;

    public BatchData(PrimitiveType primitiveType, int startIndex, int? textureHandle, int shaderHandle)
    {
        TextureHandle = textureHandle;
        ShaderHandle = shaderHandle;
        PrimitiveType = primitiveType;
        StartIndex = startIndex;
    }

    public bool Equals(PrimitiveType primitiveType, int? textureHandle, int shaderHandle)
    {
        return PrimitiveType == primitiveType &&
               TextureHandle == textureHandle &&
               ShaderHandle == shaderHandle;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(TextureHandle, ShaderHandle, (int)PrimitiveType, StartIndex);
    }
}
using System.Runtime.InteropServices;

namespace Hypercube.Graphics;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
public readonly struct RenderCommand
{
    private const int Aligiment = 8 - sizeof(LowCommandType);
    
    [FieldOffset(0)] public readonly LowCommandType Type;
    
    // [FieldOffset(Aligiment)] public readonly BindTextureCommand BindTexture;
}

public enum LowCommandType : short
{
    // Clear
    Clear,
    ClearSettings,

    // Rendering settings
    CullFaceMode,
    Scissor,
    Viewport,
    View,

    // Rendering uniforms
    Projection,
    Primitive,
    
    // Shader
    BindShader,
    UnbindShader,
    CreateShader,
    
    // Textures
    BindTexture,
    UnbindTexture,
    CreateTexture,
    
    // Uploading
    UploadVertices,
    UploadIndices,
    
    // Drawing
    Draw
}

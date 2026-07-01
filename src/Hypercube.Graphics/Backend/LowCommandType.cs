namespace Hypercube.Graphics.Backend;

public enum LowCommandType : short
{
    // Clear
    Clear,
    ClearSettings,

    CullFaceMode,
    Scissor,
    Viewport,
    View,

    // Shader
    BindShader,
    CreateShader,

    // Rendering uniforms
    Projection,
    Primitive,

    // Textures
    BindTexture,
    CreateTexture
}

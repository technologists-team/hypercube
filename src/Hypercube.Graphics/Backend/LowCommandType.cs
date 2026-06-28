namespace Hypercube.Graphics.Backend;

public enum LowCommandType : short
{
    Color,
    CullFaceMode,
    Scissor,
    Viewport,
    BindShader,
    View,
    Projection,
    Primitive,
    
    // Textures
    BindTexture,
    CreateTexture
}

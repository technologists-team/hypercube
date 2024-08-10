namespace Hypercube.ImGui.Implementations;

/// <summary>
/// A place to store various strings that contain directly written shaders.
/// Without having to use the file system to load them.
/// </summary>
internal static class ShaderSource
{
    public const string VertexShader =
    @"#version 330 core
          
    layout (location = 0) in vec2 aPos;
    layout (location = 1) in vec4 aColor;
    layout (location = 2) in vec2 aTexCoord;

    out vec4 Color;
    out vec2 TexCoord;

    uniform mat4 projection;

    void main()
    {
        gl_Position = projection * vec4(aPos.xy, 0, 1);

        Color = aColor;
        TexCoord = aTexCoord;
    }";
    
    public const string FragmentShader =
    @"#version 330 core
    
    layout (location = 0) out vec4 oColor;

    in vec4 Color;
    in vec2 TexCoord;
    
    uniform sampler2D Texture;

    void main()
    {
        oColor = Color * texture(Texture, TexCoord.st);
    }";
}
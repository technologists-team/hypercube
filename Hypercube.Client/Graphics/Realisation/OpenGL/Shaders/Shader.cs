using Hypercube.Client.Graphics.Shaders;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Shaders;

public sealed class Shader : IShader
{
    public int Handle { get; }
    public ShaderType Type { get; }

    public Shader(string source, ShaderType type)
    {
        Handle = GL.CreateShader(type);
        Type = type;
        
        GL.ShaderSource(Handle, source);
        GL.CompileShader(Handle);
    }

    public void Dispose()
    {
        GL.DeleteShader(Handle);
    }

    private void Compile()
    {
        GL.CompileShader(Handle);
        GL.GetShader(Handle, ShaderParameter.CompileStatus, out var code);
        if (code == (int)All.True)
            return;
        
        var infoLog = GL.GetShaderInfoLog(Handle);
        throw new Exception($"Error occurred whilst compiling Shader({Handle}).\n\n{infoLog}");
    }
}
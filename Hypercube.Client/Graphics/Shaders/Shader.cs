using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Shaders;

public sealed class Shader : IShader
{
    public int Handle => _handle;
    
    private readonly int _handle;
    
    private bool _disposed;

    public Shader(string path, ShaderType type)
    {
        _handle = GL.CreateShader(type);
        
        Source(File.ReadAllText(path));
        Compile();
    }

    public void Delete()
    {
        GL.DeleteShader(_handle);        
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        Delete();
        _disposed = true;
    }
    
    private void Source(string source)
    {
        GL.ShaderSource(_handle, source);
    }

    private void Compile()
    {
        GL.CompileShader(_handle);
        GL.GetShader(_handle, ShaderParameter.CompileStatus, out var code);
        
        if (code == (int)All.True)
            return;
        
        var infoLog = GL.GetShaderInfoLog(_handle);
        throw new Exception($"Error occurred whilst compiling Shader({_handle}).\n\n{infoLog}");
    }
}
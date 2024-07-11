using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using OpenToolkit.Graphics.OpenGL4;
using Vector2 = Hypercube.Shared.Math.Vector.Vector2;

namespace Hypercube.Client.Graphics.Shading;

public class Shader : IShader
{
    public readonly int _handle;
    private readonly Dictionary<string, int> _uniformLocations = new();

    public Shader(string path, IResourceManager manager) : this(new ResourcePath($"{path}.vert"), new ResourcePath($"{path}.frag"),
        manager)
    {
    }
    
    private Shader(ResourcePath vertPath, ResourcePath fragPath, IResourceManager resourceManager)
    {
        var vertSource = resourceManager.ReadFileContentAllText(vertPath);
        var fragSource = resourceManager.ReadFileContentAllText(fragPath);
        
        var vertexShader = CreateShader(vertSource, ShaderType.VertexShader);
        var fragmentShader = CreateShader(fragSource, ShaderType.FragmentShader);

        _handle = GL.CreateProgram();

        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);

        LinkProgram(_handle);

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);

        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var uniforms);
        
        _uniformLocations.Clear();
        
        for (var i = 0; i < uniforms; i++)
        {
            var key = GL.GetActiveUniform(_handle, i, out _, out _);
            var location = GL.GetUniformLocation(_handle, key);
            
            _uniformLocations.Add(key, location);
        }
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void SetUniform(string name, int value)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniformLocations[name], value);
    }
    
    public void SetUniform(string name, Vector2Int value)
    {
        GL.UseProgram(_handle);
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }
    
    public void SetUniform(string name, float value)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, Vector2 value)
    {
        GL.UseProgram(_handle);
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }
    
    private int CreateShader(string source, ShaderType type)
    {
        var shader = GL.CreateShader(type);

        GL.ShaderSource(shader, source);
        
        CompileShader(shader);

        return shader;
    }
    
    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

        if (code == (int)All.True)
            return;
        
        var infoLog = GL.GetShaderInfoLog(shader);
        throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
    }
    
    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        
        if (code == (int)All.True)
            return;
        
        throw new Exception($"Error occurred whilst linking Program({program})");
    }
    
    //private readonly record struct AttributeInfo
}
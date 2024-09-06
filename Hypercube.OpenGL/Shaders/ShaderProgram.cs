using System.Collections.Frozen;
using Hypercube.Graphics.Shaders;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;
using Hypercube.OpenGL.Utilities.Helpers;
using JetBrains.Annotations;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.OpenGL.Shaders;

[PublicAPI]
public class ShaderProgram : IShaderProgram
{
    public int Handle { get; private set; }

    private readonly FrozenDictionary<string, int> _uniformLocations;

    public ShaderProgram(string vertSource, string fragSource)
    {
        var shaders = new HashSet<IShader>
        {
            CreateShader(vertSource, ShaderType.VertexShader),
            CreateShader(fragSource, ShaderType.FragmentShader)
        };

        Handle = GL.CreateProgram();

        foreach (var shader in shaders)
        {
            Attach(shader);
        }

        LinkProgram();

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        foreach (var shader in shaders)
        {
            Detach(shader);
        }

        _uniformLocations = GetUniformLocations();
    }

    public ShaderProgram(IEnumerable<KeyValuePair<ShaderType, string>> sources)
    {
        var shaders = new HashSet<IShader>();
        foreach (var (type, shader) in sources)
        {
            shaders.Add(CreateShader(shader, type));
        }

        Handle = GL.CreateProgram();
        
        foreach (var shader in shaders)
        {
            Attach(shader);
        }

        LinkProgram();

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        foreach (var shader in shaders)
        {
            Detach(shader);
        }

        _uniformLocations = GetUniformLocations();
    }

    public ShaderProgram(HashSet<IShader> shaders)
    {

        _uniformLocations = GetUniformLocations();
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public void Stop()
    {
        GL.UseProgram(0);
    }

    public void Attach(IShader shader)
    {
        GL.AttachShader(Handle, shader.Handle);
    }

    public void Detach(IShader shader)
    {
        GL.DetachShader(Handle, shader.Handle);
    }

    public void SetUniform(string name, byte value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, short value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, int value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, long value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, float value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, double value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, Vector2 value)
    {
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }

    public void SetUniform(string name, Vector2i value)
    {
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }

    public void SetUniform(string name, Vector3 value)
    {
        GL.Uniform3(_uniformLocations[name], value.X, value.Y, value.Z);
    }

    public unsafe void SetUniform(string name, Matrix3X3 value, bool transpose = false)
    {
        var matrix = transpose ? Matrix3X3.Transpose(value) : new Matrix3X3(value);
        GL.UniformMatrix3(GL.GetUniformLocation(Handle, name), 1, false, (float*)&matrix);
    }

    public unsafe void SetUniform(string name, Matrix4X4 value, bool transpose = false)
    {
        var matrix = transpose ? Matrix4X4.Transpose(value) : new Matrix4X4(value);
        GL.UniformMatrix4(GL.GetUniformLocation(Handle, name), 1, false, (float*)&matrix);
    }

    public void Label(string name)
    {
        GLHelper.LabelObject(ObjectLabelIdentifier.Program, Handle, name);    
    }

    public void Dispose()
    {
        GL.DeleteProgram(Handle);
    }

    private FrozenDictionary<string, int> GetUniformLocations()
    {
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var uniforms);
       
        var uniformLocations = new Dictionary<string, int>();
        for (var i = 0; i < uniforms; i++)
        {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);
            var location = GL.GetUniformLocation(Handle, key);
            uniformLocations.Add(key, location);
        }

        return uniformLocations.ToFrozenDictionary();
    }
    
    private void LinkProgram()
    {
        GL.LinkProgram(Handle);
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var code);
        if (code == (int)All.True)
            return;
        
        throw new Exception($"Error occurred whilst linking Program({Handle})");
    }

    public static IShader CreateShader(string source, ShaderType type)
    {
        return new Shader(source, type);
    }
}
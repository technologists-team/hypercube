using System.Collections.Frozen;
using Hypercube.Client.Graphics.Shaders;
using Hypercube.Math.Matrices;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Shaders;

public sealed class ShaderProgram : IShaderProgram
{
    private readonly int _handle;
    private readonly FrozenDictionary<string, int> _uniformLocations;

    public ShaderProgram(string path, IResourceLoader loader) : this(new ResourcePath($"{path}.vert"), new ResourcePath($"{path}.frag"),
        loader)
    {
    }
    
    private ShaderProgram(ResourcePath vertPath, ResourcePath fragPath, IResourceLoader resourceLoader)
    {
        var shaders = new HashSet<IShader>
        {
            CreateShader(vertPath, ShaderType.VertexShader, resourceLoader),
            CreateShader(fragPath, ShaderType.FragmentShader, resourceLoader)
        };
        
        _handle = GL.CreateProgram();

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

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void Stop()
    {
        GL.UseProgram(0);
    }

    public void Attach(IShader shader)
    {
        GL.AttachShader(_handle, shader.Handle);
    }

    public void Detach(IShader shader)
    {
        GL.DetachShader(_handle, shader.Handle);
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

    public void SetUniform(string name, Vector2Int value)
    {
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }

    public void SetUniform(string name, Vector3 value)
    {
        GL.Uniform3(_uniformLocations[name], value.X, value.Y, value.Z);
    }

    public void SetUniform(string name, Matrix3X3 value, bool transpose = false)
    {
        throw new NotImplementedException();
        unsafe
        {
            //var matrix = transpose ? Matrix3X3.Transpose(value) : new Matrix3X3(value);
            //GL.UniformMatrix3(GL.GetUniformLocation(_handle, name), 1, false, (float*)&matrix);
        }
    }

    public void SetUniform(string name, Matrix4X4 value, bool transpose = false)
    {
        unsafe
        {
            var matrix = transpose ? Matrix4X4.Transpose(value) : new Matrix4X4(value);
            GL.UniformMatrix4(GL.GetUniformLocation(_handle, name), 1, false, (float*) &matrix);
        }
    }
    
    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }

    private FrozenDictionary<string, int> GetUniformLocations()
    {
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var uniforms);
       
        var uniformLocations = new Dictionary<string, int>();
        for (var i = 0; i < uniforms; i++)
        {
            var key = GL.GetActiveUniform(_handle, i, out _, out _);
            var location = GL.GetUniformLocation(_handle, key);
            uniformLocations.Add(key, location);
        }

        return uniformLocations.ToFrozenDictionary();
    }
    
    private void LinkProgram()
    {
        GL.LinkProgram(_handle);
        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out var code);
        if (code == (int)All.True)
            return;
        
        throw new Exception($"Error occurred whilst linking Program({_handle})");
    }
    
    private IShader CreateShader(ResourcePath path, ShaderType type, IResourceLoader resourceLoader)
    {
        var source = resourceLoader.ReadFileContentAllText(path);
        var shader = new Shader(source, type);
        return shader;
    }
}
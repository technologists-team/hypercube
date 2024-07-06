using System.Collections.Frozen;
using Hypercube.Math.Vector;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Shaders.Program;

public sealed class ShaderProgram : IShaderProgram
{
    private const int NullProgramHandle = 0;
    
    private readonly int _handle;
    private readonly FrozenDictionary<string, int> _uniformLocations;
    private readonly FrozenDictionary<string, AttributeInfo> _attributeInfo;
    
    private ShaderProgram(params (string, ShaderType)[] shaderDefinitions)
    {
        _handle = GL.CreateProgram();
        var shaders = new HashSet<IShader>();

        foreach (var (path, type) in shaderDefinitions)
        {
            shaders.Add(CreateShader(path, type));
        }
        
        Link();
        
        foreach (var shader in shaders)
        {
            DeleteShader(shader);
        }
        
        _uniformLocations = GetUniformLocations();
        _attributeInfo = GetAttributeLocations();
    }
    
    public ShaderProgram(string path) : this(($"{path}.vert", ShaderType.VertexShader),
        ($"{path}.frag", ShaderType.VertexShader))
    {
        // Create 2 shaders, vertex and fragment,
        // selected by file extension
    }
    
    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void Stop()
    {
        GL.UseProgram(NullProgramHandle);
    }
    
    public void SetUniform(string name, int value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }
    
    public void SetUniform(string name, Vector2Int value)
    {
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }
    
    public void SetUniform(string name, float value)
    {
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, Vector2 value)
    {
        GL.Uniform2(_uniformLocations[name], value.X, value.Y);
    }
    
    private IShader CreateShader(string path, ShaderType type)
    {
        var shader = new Shader(path, type);
        Attach(shader);

        return shader;
    }

    private void DeleteShader(IShader shader)
    {
        Detach(shader);
        shader.Delete();
    }
    
    private void Link()
    {
        GL.LinkProgram(_handle);
    }

    private void Attach(IShader shader)
    {
        GL.AttachShader(_handle, shader.Handle);
    }

    private void Detach(IShader shader)
    {
        GL.DetachShader(_handle, shader.Handle);
    }

    private void GetParameter(GetProgramParameterName parameter, out int parameters)
    {
        GL.GetProgram(_handle, parameter, out parameters);
    }

    private FrozenDictionary<string, int> GetUniformLocations()
    {
        GetParameter(GetProgramParameterName.ActiveUniforms, out var uniforms);
        
        var uniformLocations = new Dictionary<string, int>();
        for (var i = 0; i < uniforms; i++)
        {
            var key = GL.GetActiveUniform(_handle, i, out _, out _);
            var location = GL.GetUniformLocation(_handle, key);
            
            uniformLocations.Add(key, location);
        }

        return uniformLocations.ToFrozenDictionary();
    }

    private FrozenDictionary<string, AttributeInfo> GetAttributeLocations()
    {
        GetParameter(GetProgramParameterName.ActiveAttributes, out var attributes);
        
        var attributeLocations = new Dictionary<string, AttributeInfo>();
        for (var i = 0; i < attributes; i++)
        {
            var key = GL.GetActiveAttrib(_handle, i, out var size, out var type);
            var location = GL.GetAttribLocation(_handle, key);
            
            attributeLocations.Add(key, new AttributeInfo(location, size, type));
        }

        return attributeLocations.ToFrozenDictionary();
    }
    
    private readonly record struct AttributeInfo(int Location, int Size, ActiveAttribType Type);
}
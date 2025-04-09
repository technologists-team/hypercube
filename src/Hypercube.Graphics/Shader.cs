using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;
using Hypercube.Resources.Loaders;

namespace Hypercube.Graphics;

/// <remarks>
/// This class acts as a wrapper around an underlying <see cref="IShaderProgram"/> implementation.
/// </remarks>
public sealed class Shader : Resource, IShaderProgram
{
    private readonly IShaderProgram _program;
    
    public uint Handle => _program.Handle;
    
    public Shader(IShaderProgram program)
    {
        _program = program;
    }

    public void Use()
    {
        _program.Use();
    }

    public void Stop()
    {
        _program.Stop();
    }

    public void SetUniform(string name, int value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, float value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, double value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, Vector2 value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, Vector2i value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, Vector3 value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, Vector3i value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, Vector4 value)
    {
        _program.SetUniform(name, value);
    }

    public void SetUniform(string name, Matrix3x3 value, bool transpose = false)
    {
        _program.SetUniform(name, value, transpose);
    }

    public void SetUniform(string name, Matrix4x4 value, bool transpose = false)
    {
        _program.SetUniform(name, value, transpose);
    }

    public void SetUniform(string name, Color value)
    {
        _program.SetUniform(name, value);
    }

    public void Label(string name)
    {
        _program.Label(name);
    }

    public override void Dispose()
    {
        _program.Dispose();
    }
}
using Hypercube.Math.Matrices;
using Hypercube.Math.Vectors;

namespace Hypercube.Client.Graphics.Shaders;

/// <summary>
/// What is usually called just a shader,
/// when created creates a fragment and vertex shader,
/// which are <see cref="Attach"/> and dispose afterwards.
/// </summary>
public interface IShaderProgram : IDisposable
{
    int Handle { get; }
    void Use();
    void Stop();

    void Attach(IShader shader);
    void Detach(IShader shader);
        
    void SetUniform(string name, byte value);
    void SetUniform(string name, short value);
    void SetUniform(string name, int value);
    void SetUniform(string name, long value);
    void SetUniform(string name, float value);
    void SetUniform(string name, double value);
    
    void SetUniform(string name, Vector2 value);
    void SetUniform(string name, Vector2Int value);
    void SetUniform(string name, Vector3 value);
    
    void SetUniform(string name, Matrix3X3 value, bool transpose = false);
    void SetUniform(string name, Matrix4X4 value, bool transpose = false);
}
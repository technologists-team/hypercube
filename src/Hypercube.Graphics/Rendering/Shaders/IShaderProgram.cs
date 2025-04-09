using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Rendering.Shaders;

public interface IShaderProgram : IDisposable
{
    /// <summary>
    /// Gets the handle for this shader program.
    /// </summary>
    uint Handle { get; }
    
    void Use();
    void Stop();
    
    void SetUniform(string name, int value);
    void SetUniform(string name, float value);
    void SetUniform(string name, double value);
    void SetUniform(string name, Vector2 value);
    void SetUniform(string name, Vector2i value);
    void SetUniform(string name, Vector3 value);
    void SetUniform(string name, Vector3i value);
    void SetUniform(string name, Vector4 value);
    void SetUniform(string name, Matrix3x3 value, bool transpose = false);
    void SetUniform(string name, Matrix4x4 value, bool transpose = false);
    void SetUniform(string name, Color value);

    void Label(string name);
}
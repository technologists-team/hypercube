using Hypercube.Math.Vector;

namespace Hypercube.Client.Graphics.Shaders.Program;

public interface IShaderProgram
{
    void Use();
    void SetUniform(string name, int value);
    void SetUniform(string name, float value);
    void SetUniform(string name, Vector2 value);
    void SetUniform(string name, Vector2Int value);
}
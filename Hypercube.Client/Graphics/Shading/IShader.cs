using Hypercube.Shared.Math.Matrix;

namespace Hypercube.Client.Graphics.Shading;

public interface IShader
{
    void Use();
    int GetUniformLocation(string name);
    void SetUniform(string name, int value);
    void SetUniform(string name, Matrix4X4 value, bool transpose = false);
    void SetUniform(int index, Matrix4X4 value, bool transpose = false);
}
namespace Hypercube.Client.Graphics.Shading;

public interface IShader
{
    void Use();
    void SetUniform(string name, int value);
}
namespace Hypercube.Client.Graphics.Shaders;

/// <summary>
/// Part of the shader <see cref="IShaderProgram"/>, basically
/// a fragment or vertex shader.
/// </summary>
public interface IShader : IDisposable
{
    int Handle { get; }
}
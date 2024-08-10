using JetBrains.Annotations;

namespace Hypercube.Graphics.Shaders;

/// <summary>
/// Part of the shader <see cref="IShaderProgram"/>, basically
/// a fragment or vertex shader.
/// </summary>
[PublicAPI]
public interface IShader : IDisposable
{
    int Handle { get; }
}
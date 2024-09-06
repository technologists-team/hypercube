using JetBrains.Annotations;

namespace Hypercube.Graphics.Shaders.Exceptions;

[PublicAPI]
public class ShaderCompilationException : Exception
{
    public ShaderCompilationException(int handle, string infoLog) : base($"Error occurred whilst compiling shader: {handle}.\n\n{infoLog}")
    {
    }
}
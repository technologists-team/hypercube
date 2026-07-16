using Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    private OpenGlArrayObject GenArrayObject() => new(_gl);

    private OpenGlArrayObject GenArrayObject(string label) => new(_gl, label);

    private OpenGlBufferObject GenBufferObject(BufferTargetARB target) => new(_gl, target);

    private OpenGlBufferObject GenBufferObject(BufferTargetARB target, string label) => new(_gl, target, label);
}

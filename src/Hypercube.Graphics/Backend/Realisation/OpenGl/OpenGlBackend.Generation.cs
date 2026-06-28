using Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    public OpenGlArrayObject GenArrayObject() => new(_gl);

    public OpenGlArrayObject GenArrayObject(string label) => new(_gl, label);

    public OpenGlBufferObject GenBufferObject(BufferTargetARB target) => new(_gl, target);

    public OpenGlBufferObject GenBufferObject(BufferTargetARB target, string label) => new(_gl, target, label);
}

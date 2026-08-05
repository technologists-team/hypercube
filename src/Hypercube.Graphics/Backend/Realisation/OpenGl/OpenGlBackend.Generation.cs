using Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    private OpenGlArrayObject GenArrayObject() => new(Gl);

    private OpenGlArrayObject GenArrayObject(string label) => new(Gl, label);

    private OpenGlBufferObject GenBufferObject(BufferTargetARB target) => new(Gl, target);

    private OpenGlBufferObject GenBufferObject(BufferTargetARB target, string label) => new(Gl, target, label);
}

using System.Diagnostics;
using JetBrains.Annotations;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;

[DebuggerDisplay("VertexArray {_handle}")]
public sealed class OpenGlArrayObject : IDisposable
{
    private const uint Null = 0;

    private readonly GL _gl;
    private readonly uint _handle;
        
    private bool _bound;

    public OpenGlArrayObject(GL gl)
    {
        _gl = gl;
        _handle = _gl.GenVertexArray();
    }

    public OpenGlArrayObject(GL gl, string label) : this(gl)
    {
        Label(label);
    }
        
    [PublicAPI]
    public void Bind()
    {
        _gl.BindVertexArray(_handle);
    }

    [PublicAPI]
    public void Unbind()
    {
        _gl.BindVertexArray(Null);
    }

    [PublicAPI]
    public void Delete()
    {
        _gl.DeleteVertexArray(_handle);
    }

    [PublicAPI]
    public void Label(string name)
    {
        Bind();
        _gl.ObjectLabel(ObjectIdentifier.VertexArray, _handle, name);
    }

    public void Dispose()
    {
        Delete();
    }
}
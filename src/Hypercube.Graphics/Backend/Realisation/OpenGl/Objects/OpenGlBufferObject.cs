using System.Diagnostics;
using JetBrains.Annotations;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;

[DebuggerDisplay("Buffer {_handle} ({BufferTargetName})")]
public sealed class OpenGlBufferObject : IDisposable
{
    private const uint Null = 0;

    private readonly GL _gl;
    private readonly uint _handle;
    private readonly BufferTargetARB _target;
        
    private bool _bound;

    private string BufferTargetName => Enum.GetName(_target) ?? _target.ToString();

    public OpenGlBufferObject(GL gl, BufferTargetARB target)
    {
        _gl = gl;
        _handle = _gl.GenBuffer();
        _target = target;
    }

    public OpenGlBufferObject(GL gl, BufferTargetARB target, string label) : this(gl, target)
    {
        Label(label);
    }
    
    [PublicAPI]
    public void Bind()
    {
        if (_bound)
            return;
    
        _bound = true;
        _gl.BindBuffer(_target, _handle);
    }
    
    [PublicAPI]
    public void Unbind()
    {
        if (!_bound)
            return;
    
        _bound = false;
        _gl.BindBuffer(_target, Null);
    }
    
    [PublicAPI]
    public void Delete()
    {
        _gl.DeleteBuffer(_handle);
    }
        
    [PublicAPI]
    public unsafe void SetData(int size, nint data, BufferUsageARB hint = BufferUsageARB.StaticDraw)
    {
        Bind();
        _gl.BufferData(_target, (nuint) size, (void*) data, hint);
    }

    [PublicAPI]
    public unsafe void SetData<T>(T[] data, BufferUsageARB hint = BufferUsageARB.StaticDraw) where T : unmanaged
    {
        Bind();
        fixed (T* dataPtr = data)
            _gl.BufferData(_target, (nuint) (data.Length * sizeof(T)), dataPtr, hint);
    }

    [PublicAPI]
    public unsafe void SetSubData(int size, nint data)
    {
        Bind();
        throw new NotImplementedException();
    }

    [PublicAPI]
    public void Label(string name)
    {
        Bind();
        _gl.ObjectLabel(ObjectIdentifier.Buffer, _handle, name);
    }

    public void Dispose()
    {
        Delete();
    }
}